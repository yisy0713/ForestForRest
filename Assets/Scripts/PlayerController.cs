using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int jumpCount = 1;
    public int CurrJumpCount = 0;

    public float walkSpeed = 9f;
    public float runSpeed = 15f;
    private float applySpeed;       // walkSpeed나 runSpeed 적용

    private float jumpForce = 5f;

    public float runningStamina = 0.2f;
    public float jumpingStamina = 30f;

    public bool isStop = true;
    public bool isRun = false;
    bool isGround = true;

    private CapsuleCollider capsuleCollider;

    public float lookSensitivity;

    public float cameraRotationLimit;
    private float currentCameraRotationX = 0f;

    public Camera theCamera;
    private Rigidbody myRigid;
    private StatusUI statusController;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        statusController = FindObjectOfType<StatusUI>();
        applySpeed = walkSpeed;
    }

    void Update()
    {
        if (!PlayerIsDead())
        {
            IsGround();
            TryJump();
            TryRun();       // 뛰는지 확인 (무조건 Move()함수 위에 위치해야함)
            Move();

            if (!Inventory.inventoryActivated && !Map.MapActivated)
            {
                CameraRotation();
                CharacterRotation();
            }
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
        {
            isStop = false;
        }
        else if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W))
        {
            isStop = true;
        }
    }

    public bool PlayerIsDead()
    {
        return statusController.GetIsDead();
    }

    private void IsGround()
    {
        bool wasGround = isGround;
        isGround = Physics.Raycast(transform.position, -transform.up, capsuleCollider.bounds.extents.y + 0.1f);

        if (isGround && !wasGround)
        {
            CurrJumpCount = 0;
        }
    }

    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) /*&& isGround*/ && statusController.GetCurSp() > 0 && (CurrJumpCount < jumpCount))
        {
            Jump();
            CurrJumpCount ++;
        }
    }
    private void Jump()
    {
        myRigid.velocity = transform.up * jumpForce;
        statusController.DecreaseJumpStamina(jumpingStamina);
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && statusController.GetCurSp() > 0)
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || statusController.GetCurSp() <= 0)
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        isRun = true;
        applySpeed = runSpeed;
        statusController.DecreaseStamina(runningStamina);
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");           // 오른쪽 : 1 , 왼쪽 : -1, null : 0
        float _moveDirZ = Input.GetAxisRaw("Vertical");             // 앞 : 1 , 뒤 : -1, null : 0

        transform.Translate(new Vector3(_moveDirX, 0, _moveDirZ) * Time.deltaTime * applySpeed);    // 물리 무시 이동

        //Vector3 _moveHorizontal = transform.right * _moveDirX;        
        //Vector3 _moveVertical = transform.forward * _moveDirZ; 
        //Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
        //myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);        // 물리적 기반 이동 (terrain이 있을때와 없을때 이동속도 차이가 남)
    }

    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    // 패시브 아이템 관련 함수

    public void IncreaseJumpForce(float count)
    {
        jumpForce += count;
    }

    public void IncreaseMoveSpeed(float count)
    {
        walkSpeed += count * 0.5f;
        runSpeed += count;
    }

    public void DecreaseStemiaUse(float count)
    {
        if(runningStamina - count >= 20)
        {
            runningStamina -= count;
        }
        else
        {
            runningStamina = 20;
        }

        if (jumpingStamina - count >= 15f)
        {
            jumpingStamina -= count;
        }
        else
        {
            jumpingStamina = 15f;
        }
    }

    public void IncreaseJumpCount(int count = 1)
    {
        jumpCount = jumpCount + count;
    }
}
