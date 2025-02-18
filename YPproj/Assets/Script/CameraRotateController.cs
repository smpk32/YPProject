
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraRotateController : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Transform player = null;
    public PlayerController pc;
    private Transform playerCam = null;
    private float rotationSpeed = 1f;

    Vector3 beginPos;
    Vector3 draggingPos;

    float xAngle;
    float yAngle;
    float camXAngle;
    float camYAngle;
    float xAngleTemp;
    float yAngleTemp;
    float camXAngleTemp; 
    float camYAngleTemp;
  

    private float camera_dist = 0f; //리그로부터 카메라까지의 거리
    public float camera_width = -8.5f; //가로거리
    public float camera_height = 1.5f; //세로거리
    public float camera_fix = 1.5f;//레이케스트 후 리그쪽으로 올 거리

    Vector3 dir; //

    bool dragging = false;

    RaycastHit hit;
    Ray ray;
    private void Start()
    {
        if(player != null)
        {
            Init();

            CancelInvoke("SetCameraRotPlayer");
        }
        else
        {
            InvokeRepeating("SetCameraRotPlayer", 0f, 0.25f);
        }


        //pc = gameObject.GetComponent<PlayerController>();

        //Init();
        /*dir = new Vector3(0, camera_height, camera_width).normalized;
        camera_dist = Mathf.Sqrt(camera_width * camera_width + camera_height * camera_height);

        xAngle = player.rotation.eulerAngles.x;
        yAngle = player.rotation.eulerAngles.y;

        // 카메라 세팅
        playerCam = GameObject.Find("PlayerObj").transform.Find("CameraObj").transform;
        camXAngle = playerCam.rotation.eulerAngles.x;
        camYAngle = playerCam.rotation.eulerAngles.y; */

    }

    public void SetCameraRotPlayer()
    {
        if (player == null && GameManager.instance.playerPrefab != null)
        {
            //Debug.Log(GameManager.instance.playerPrefab.transform);
            player = GameManager.instance.playerPrefab.transform;

            if (player != null)
            {
                Init();
                CancelInvoke("SetCameraRotPlayer");
            }
        }
    }

    void Update()
    {

        //SetCameraRotPlayer();
        
    }

    public void Init()
    {
        dir = new Vector3(0, camera_height, camera_width).normalized;
        camera_dist = Mathf.Sqrt(camera_width * camera_width + camera_height * camera_height);

        xAngle = player.rotation.eulerAngles.x;
        yAngle = player.rotation.eulerAngles.y;

        // 카메라 세팅
        playerCam = player.transform.Find("CameraObj").transform;
        camXAngle = playerCam.rotation.eulerAngles.x;
        camYAngle = playerCam.rotation.eulerAngles.y;

        if (GameManager.instance.multiState.Equals("Multi"))
        {
            GameObject.Find("MainCanvas").gameObject.transform.Find("LoadingImage").gameObject.SetActive(false);
        }


    }

    //드래그 시작전
    public void OnBeginDrag(PointerEventData beginPoint)
    {

        dragging = true;
        beginPos = beginPoint.position;

        xAngleTemp = xAngle;
        yAngleTemp = yAngle;

        camXAngleTemp = camXAngle;
        camYAngleTemp = camYAngle;
    }

    //드래깅 
    public void OnDrag(PointerEventData draggingPoint)
    {
        // 자동이동중에도 카메라 회전 가능하게 하위 코드 임시 주석처리
        if(GameManager.instance.playerState == PlayerState.setting)
        {
            return;
        }

        draggingPos = draggingPoint.position;

        yAngle = yAngleTemp + (draggingPos.x - beginPos.x) * 180 / Screen.width * rotationSpeed;
        xAngle = xAngleTemp - (draggingPos.y - beginPos.y) * 90 / Screen.height * rotationSpeed;

        camYAngle = camYAngleTemp + (draggingPos.x - beginPos.x) * 180 / Screen.width * rotationSpeed;
        camXAngle = camXAngleTemp - (draggingPos.y - beginPos.y) * 90 / Screen.height * rotationSpeed;

        if (camXAngle > 30)
        {
            camXAngle = 30;
        }
        if (camXAngle < -30)
        {
            camXAngle = -30;
        }

        playerCam.rotation = Quaternion.Euler(camXAngle, camYAngle, 0.0f);
    
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (dragging)
        {
            dragging = false;
            return;
        }
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "EvtBtn")
            {
                if (GameManager.instance.playerState != PlayerState.setting)
                {

                    hit.transform.GetComponent<Button>().onClick.Invoke();
                }

            }
        }
    }


    void LateUpdate()
    {
        SetObjectOverCamera();
    }

    void SetObjectOverCamera()
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));

        if (playerCam == null)
        {
            return;
        }

        Transform cp = Camera.main.transform;

        Vector3 ray_target = playerCam.up * camera_height + playerCam.forward * camera_width;

        RaycastHit hitinfo;

        Physics.Raycast(playerCam.position, ray_target,  out hitinfo, camera_dist, layerMask);
      
        if (hitinfo.point != Vector3.zero)//레이케스트 성공시
        {           
            //point로 옮긴다.
            cp.position = hitinfo.point;
            //카메라 보정
            cp.Translate(dir * -1 * camera_fix);
        }
        else
        {
           
            //로컬좌표를 0으로 맞춘다. (카메라리그로 옮긴다.)
            cp.localPosition = Vector3.zero;
            //카메라위치까지의 방향벡터 * 카메라 최대거리 로 옮긴다.
            cp.Translate(dir * camera_dist);
            //카메라 보정
            cp.Translate(dir * -1 * camera_fix);

        }
    }

}