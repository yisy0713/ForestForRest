using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ���ǵ� ���� ����
    public float walkSpeed = 9f;
    public float runSpeed = 15f;
    private float applySpeed;       // walkSpeed �Ǵ� runSpeed�� ����

    private float jumpForce = 5f;

    public float runningStamina = 0.2f;
    public float jumpingStamina = 30f;

    // ���� ����
    public bool isStop = true;
    public bool isRun = false;
    bool isGround = true;

    // �� ���� ����
    private CapsuleCollider capsuleCollider;

    // �ΰ���
    public float lookSensitivity;

    // ī�޶� �Ѱ�
    public float cameraRotationLimit;
    private float currentCameraRotationX = 0f;

    // �ʿ��� ������Ʈ
    public Camera theCamera;
    private Rigidbody myRigid;
    private StatusUI statusController;

    public GameObject GreenBoss;        // �׺�޽�����׿�

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
            TryRun();       // �ٴ��� Ȯ�� (������ Move()�Լ� ���� ��ġ�ؾ���)
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

        if (Input.GetKey(KeyCode.Z))        // �׺�޽� ����׿�
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
        float _moveDirX = Input.GetAxisRaw("Horizontal");           // ������ : 1 , ���� : -1, null : 0
        float _moveDirZ = Input.GetAxisRaw("Vertical");             // �� : 1 , �� : -1, null : 0

        transform.Translate(new Vector3(_moveDirX, 0, _moveDirZ) * Time.deltaTime * applySpeed);    // ���� ���� �̵�

        //Vector3 _moveHorizontal = transform.right * _moveDirX;        
        //Vector3 _moveVertical = transform.forward * _moveDirZ; 
        //Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
        //myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);        // ������ ��� �̵� (terrain�� �������� ������ �̵��ӵ� ���̰� ��)
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

    // �нú� ������ ���� �Լ�

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
