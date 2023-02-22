
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraRotateController : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Transform player;
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
  

    private float camera_dist = 0f; //���׷κ��� ī�޶������ �Ÿ�
    public float camera_width = -8.5f; //���ΰŸ�
    public float camera_height = 1.5f; //���ΰŸ�
    public float camera_fix = 1.5f;//�����ɽ�Ʈ �� ���������� �� �Ÿ�

    Vector3 dir; //

    bool dragging = false;

    RaycastHit hit;
    Ray ray;
    private void Start()
    {
        pc = player.transform.Find("Player").GetComponent<PlayerController>();
        Init();
        /*dir = new Vector3(0, camera_height, camera_width).normalized;
        camera_dist = Mathf.Sqrt(camera_width * camera_width + camera_height * camera_height);

        xAngle = player.rotation.eulerAngles.x;
        yAngle = player.rotation.eulerAngles.y;

        // ī�޶� ����
        playerCam = GameObject.Find("PlayerObj").transform.Find("CameraObj").transform;
        camXAngle = playerCam.rotation.eulerAngles.x;
        camYAngle = playerCam.rotation.eulerAngles.y; */

    }

    public void Init()
    {
        dir = new Vector3(0, camera_height, camera_width).normalized;
        camera_dist = Mathf.Sqrt(camera_width * camera_width + camera_height * camera_height);

        xAngle = player.rotation.eulerAngles.x;
        yAngle = player.rotation.eulerAngles.y;

        // ī�޶� ����
        playerCam = GameObject.Find("PlayerObj").transform.Find("CameraObj").transform;
        camXAngle = playerCam.rotation.eulerAngles.x;
        camYAngle = playerCam.rotation.eulerAngles.y;
    }

    //�巡�� ������
    public void OnBeginDrag(PointerEventData beginPoint)
    {

        dragging = true;
        beginPos = beginPoint.position;

        xAngleTemp = xAngle;
        yAngleTemp = yAngle;

        camXAngleTemp = camXAngle;
        camYAngleTemp = camYAngle;
    }

    //�巡�� 
    public void OnDrag(PointerEventData draggingPoint)
    {
        if(pc.playerState == PlayerState.nav)
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
                hit.transform.GetComponent<Button>().onClick.Invoke();
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
      
        if (hitinfo.point != Vector3.zero)//�����ɽ�Ʈ ������
        {           
            //point�� �ű��.
            cp.position = hitinfo.point;
            //ī�޶� ����
            cp.Translate(dir * -1 * camera_fix);
        }
        else
        {
           
            //������ǥ�� 0���� �����. (ī�޶󸮱׷� �ű��.)
            cp.localPosition = Vector3.zero;
            //ī�޶���ġ������ ���⺤�� * ī�޶� �ִ�Ÿ� �� �ű��.
            cp.Translate(dir * camera_dist);
            //ī�޶� ����
            cp.Translate(dir * -1 * camera_fix);

        }
    }

}