using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //���ǵ� ���� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed;

    [SerializeField]
    private float jumpForce;

    //���º���
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    // �ɾ����� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    //�� ���� ����
    private CapsuleCollider capsuleCollider;

    //�ΰ���
    [SerializeField]
    private float lookSensitivity;

    //ī�޶��Ѱ�
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX= 0;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;

        //�ʱ�ȭ
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void TryCrouch()//�ɱ�õ�
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()//�ɱ⵿��
    {
        isCrouch = !isCrouch;
        if(isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()//�ε巯�� �ɱ� ����
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }

    

    private void IsGround()//����üũ
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryJump()//�����õ�
    {
        if(Input.GetKeyDown(KeyCode.Space)&& isGround)
        {
            Jump();
        }
    }

    private void Jump()//����
    {
        //���� ���¿��� ������ ���� ���� ����
        if (isCrouch)
            Crouch();
        myRigid.velocity = transform.up *jumpForce;
    } 
    private void TryRun()//�޸��� �õ�
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancle();
        }
    }

    private void Running()//�޸��� ����
    {
        if (isCrouch)
            Crouch();
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancle()//�޸��� ���
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()//������ ����
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void CharacterRotation() // �¿� ī�޶� ȸ��
    {
        float _yRotation = Input.GetAxisRaw("MouseX");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    private void CameraRotation()//���� ī�޶� ȸ�� 
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

}
