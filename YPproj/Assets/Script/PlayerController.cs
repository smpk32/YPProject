using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum PlayerState
{
    normal,
    sitting,
    nav
}
public class PlayerController : MonoBehaviour
{

    private float moveSpeed = 5f; // �յ� �������� �ӵ�
    public float rotateSpeed = 0.1f; // �¿� ȸ�� �ӵ�
    private float jumpForce = 5f;

    bool isJump;
    private GameObject cameraObj;
    public Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����
    
    public Rigidbody Rigidbody; // �÷��̾� ĳ������ ������ٵ�

    public string Vertical = "Vertical"; // �յ� �������� ���� �Է��� �̸�
    public string Horizontal = "Horizontal"; // �¿� ȸ���� ���� �Է��� �̸�

    public float rayDistance = 100.0f;

 
    public float x;
    public float z;

    public PlayerState playerState = PlayerState.normal;
    GameObject playerObj;

    
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerObj = GameObject.Find("PlayerObj");
        cameraObj = playerObj.transform.Find("CameraObj").transform.gameObject;
    }

    private void FixedUpdate()
    {

        if(playerState == PlayerState.sitting)
        {
            return;
        }
        // move�� ���� �Է� ����
        z = Input.GetAxis(Vertical);
        // rotate�� ���� �Է� ����
        x = Input.GetAxis(Horizontal);

        //����
        Jump();

        if (x != 0 || z != 0)
        {   
            //
            Vector3 lookForward = new Vector3(cameraObj.transform.forward.x , 0f, cameraObj.transform.forward.z ).normalized;
            Vector3 lookRight = new Vector3(cameraObj.transform.right.x, 0f, cameraObj.transform.right.z).normalized;

            //ī�޶� ���Ⱚ�� Ű�Է°� 

            Vector2 moveInput = new Vector2(x, z);
            Vector3  moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            //�ִϸ��̼� 
            playerAnimator.SetFloat("Y", x);
            playerAnimator.SetFloat("X", z);
            MoveAndRoatate(moveDir);
            FreezeRotationXZ();
        }

    }
    private void FreezeRotationXZ()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); //������ ����
    }
    void MoveAndRoatate(Vector3 _moveDir)
    {
       //ĳ���� �̵�
       Rigidbody.position += _moveDir * moveSpeed * Time.deltaTime;
        //ȸ��
       Quaternion newRotation = Quaternion.LookRotation(_moveDir);
       transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 0.2f);
    }


    void Jump()
    { // �����̽��ٸ� ������
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isJump)
            {

                Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                playerAnimator.SetTrigger("Jump");

                isJump = true;

                Invoke("ResetJump", 1.5f);

            }
        }
    }
    void ResetJump()
    {
        isJump = false;
    }

    public void Sit(bool chk, Vector3 chairPos, Vector3 chairRot)
    {
        if (chk)
        {
            playerState = PlayerState.sitting;
            playerObj.transform.position = chairPos;
            gameObject.transform.rotation = Quaternion.Euler(chairRot);
            cameraObj.transform.rotation = Quaternion.Euler(chairRot+ new Vector3(15,0,0));
            GameObject.Find("DragPanel").GetComponent<CameraRotateController>().Init();
            Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            playerAnimator.SetBool("Sit", true);
        }
        else
        {
            playerState = PlayerState.normal;
            Rigidbody.constraints = RigidbodyConstraints.None;
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            GameObject.Find("DragPanel").GetComponent<CameraRotateController>().Init();
            playerAnimator.SetBool("Sit", false);
        }
        
    }

}
