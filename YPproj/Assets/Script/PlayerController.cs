using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum PlayerState
{
    normal,
    sitting,
    nav,
    chat,
    setting
}
public class PlayerController : MonoBehaviourPun
{

    private float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 0.1f; // 좌우 회전 속도
    private float jumpForce = 5f;

    bool isJump;
    private GameObject cameraObj;
    public Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    
    public Rigidbody Rigidbody; // 플레이어 캐릭터의 리지드바디

    public string Vertical = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string Horizontal = "Horizontal"; // 좌우 회전을 위한 입력축 이름

    public float rayDistance = 100.0f;

 
    public float x;
    public float z;

    public PlayerState playerState = PlayerState.normal;
    GameObject playerObj;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerObj = gameObject.transform.parent.gameObject;
        cameraObj = playerObj.transform.Find("CameraObj").transform.gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        /*playerAnimator = GetComponent<Animator>();
        playerObj = gameObject.transform.parent.gameObject;
        Debug.Log(playerObj);
        cameraObj = playerObj.transform.Find("CameraObj").transform.gameObject;*/
    }

    private void FixedUpdate()
    {
        if(GameManager.instance.multiState == "Multi")
        {
            if (photonView!=null && !photonView.IsMine)
            {
                return;
            }
        }

        /*if (playerState == PlayerState.sitting || playerState == PlayerState.chat || playerState == PlayerState.setting)
        {
            return;
        }*/

        if(GameManager.instance.playerState != PlayerState.normal)
        {
            return;
        }
        // move에 관한 입력 감지
        z = Input.GetAxis(Vertical);
        // rotate에 관한 입력 감지
        x = Input.GetAxis(Horizontal);

        //점프
        Jump();

        if (x != 0 || z != 0)
        {   
            //
            Vector3 lookForward = new Vector3(cameraObj.transform.forward.x , 0f, cameraObj.transform.forward.z ).normalized;
            Vector3 lookRight = new Vector3(cameraObj.transform.right.x, 0f, cameraObj.transform.right.z).normalized;

            //카메라 방향값과 키입력값 

            Vector2 moveInput = new Vector2(x, z);
            Vector3  moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            //애니메이션 
            playerAnimator.SetFloat("Y", x);
            playerAnimator.SetFloat("X", z);
            MoveAndRoatate(moveDir);
            FreezeRotationXZ();
        }
        else
        {
            if(playerAnimator.GetFloat("Y") != 0 || playerAnimator.GetFloat("X") != 0)
            {
                playerAnimator.SetFloat("Y", 0);
                playerAnimator.SetFloat("X", 0);
            }
        }

    }
    private void FreezeRotationXZ()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); //기울어짐 방지
    }
    void MoveAndRoatate(Vector3 _moveDir)
    {
       //캐릭터 이동
       Rigidbody.position += _moveDir * moveSpeed * Time.deltaTime;
        //회전
       Quaternion newRotation = Quaternion.LookRotation(_moveDir);
       transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 0.2f);
    }


    void Jump()
    { // 스페이스바를 누르면
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

    public void Sit(bool chk, Vector3 chairPos, Vector3 chairRot, string chairNm)
    {
        if (chk)
        {
            GameManager.instance.playerState = PlayerState.sitting;
            playerObj.transform.position = chairPos;
            gameObject.transform.rotation = Quaternion.Euler(chairRot);
            cameraObj.transform.rotation = Quaternion.Euler(chairRot+ new Vector3(15,0,0));
            GameObject.Find("DragPanel").GetComponent<CameraRotateController>().Init();
            Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            playerAnimator.SetBool("Sit", true);
        }
        else
        {
            GameManager.instance.playerState = PlayerState.normal;
            playerObj.transform.position = new Vector3(chairPos.x, chairPos.y + 0.5f, chairPos.z);
            Rigidbody.constraints = RigidbodyConstraints.None;
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            GameObject.Find("DragPanel").GetComponent<CameraRotateController>().Init();
            playerAnimator.SetBool("Sit", false);
        }
        
    }

    [PunRPC]
    public void Sit2(int viewID, bool chk, Vector3 chairPos, Vector3 chairRot)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PhotonView>().ViewID == viewID)
            {
                if (chk)
                {
                    players[i].transform.parent.transform.position = chairPos;
                    players[i].transform.parent.position = chairPos;
                    //gameObject.transform.rotation = Quaternion.Euler(chairRot);
                    players[i].transform.rotation = Quaternion.Euler(chairRot);

                    //cameraObj.transform.rotation = Quaternion.Euler(chairRot + new Vector3(15, 0, 0));
                    //GameObject.Find("DragPanel").GetComponent<CameraRotateController>().Init();
                    players[i].transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    players[i].GetComponent<Animator>().SetBool("Sit", true);
                }
                else
                {
                    //playerState = PlayerState.normal;
                    players[i].transform.parent.position = new Vector3(chairPos.x, chairPos.y +0.5f, chairPos.z) ;
                    players[i].transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    players[i].transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                    //GameObject.Find("DragPanel").GetComponent<CameraRotateController>().Init();
                    players[i].GetComponent<Animator>().SetBool("Sit", false);

                }
            }
        }

        

    }

    public void SitEvent(bool chk, Vector3 chairPos, Vector3 chairRot)
    {
        photonView.RPC("Sit2", RpcTarget.AllBuffered, photonView.ViewID,chk,chairPos,chairRot);
    }

}
