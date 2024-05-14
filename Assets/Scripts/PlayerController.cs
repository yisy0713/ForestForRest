using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 스피드 조정 변수
    public float walkSpeed = 9f;
    public float runSpeed = 15f;
    private float applySpeed;       // walkSpeed 또는 runSpeed를 대입

    private float jumpForce = 5f;

    public float runningStamina = 0.2f;
    public float jumpingStamina = 30f;

    // 상태 변수
    public bool isStop = true;
    public bool isRun = false;
    bool isGround = true;

    // 땅 착지 여부
    private CapsuleCollider capsuleCollider;

    // 민감도
    public float lookSensitivity;

    // 카메라 한계
    public float cameraRotationLimit;
    private float currentCameraRotationX = 0f;

    // 필요한 컴포넌트
    public Camera theCamera;
    private Rigidbody myRigid;
    private StatusUI statusController;

    public GameObject GreenBoss;        // 네비메쉬디버그용

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        statusController = FindObjectOfType<StatusUI>();
        applySpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerIsDead())
        {
            IsGround();
            TryJump();
            TryRun();       // 뛰는지 확인 (무조건 Move()함수 위에 위치해야함)
            Move();

            if (!Inventory.inventoryActivated)
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

        if (Input.GetKey(KeyCode.Z))        // 네비메쉬 디버그용
        {
            GreenBoss.SetActive(true);
        }
    }

    public bool PlayerIsDead()
    {
        return statusController.GetIsDead();
    }

    private void Jump()
    {
        myRigid.velocity = transform.up * jumpForce;
        statusController.DecreaseJumpStamina(jumpingStamina);
    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, -transform.up, capsuleCollider.bounds.extents.y + 0.1f);
        
        /*if(isGround)
            Debug.Log("isGround!");*/
    }

    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround && statusController.GetCurSp() > 0)
        {
            Jump();
        }
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

        if (jumpingStamina - count >= 150f)
        {
            jumpingStamina -= count;
        }
        else
        {
            jumpingStamina = 150f;
        }
    }
}
