using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    Transform player;
    RaycastHit hit;
    Ray ray;

    private float rotationSpeed = 1f;

    Vector3 beginPos;
    Vector3 draggingPos;

    float xAngleTemp;
    float yAngleTemp;
    float xAngle;
    float yAngle;


    bool dragging = false;

    float camera_dist = 0f;

    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        xAngle = transform.rotation.eulerAngles.x;
        yAngle = transform.rotation.eulerAngles.y;

        player = GameObject.Find("Player").transform;
        

        camera_dist = Vector3.Distance(player.transform.position, cube.transform.position)/2;
        Debug.Log(camera_dist);

    }

    void LateUpdate()
    {
        SetObjectOverCamera();
    }

    void SetObjectOverCamera()
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));

        GameObject cam = Camera.main.gameObject;
        Vector3 ray_target = player.localRotation* Vector3.forward;

        RaycastHit hitinfo;
        //Debug.Log(ray_target);

        Physics.Raycast(player.position, ray_target, out hitinfo, camera_dist, layerMask);
        Debug.DrawRay(player.position, ray_target* camera_dist, Color.red);
        //Vector3 ray_target = player.up * camera_height + player.forward * camera_width;

        if(hitinfo.point != Vector3.zero)
        {
            Debug.Log(hitinfo.point);
        }

    }

    public void OnBeginDrag(PointerEventData beginPoint)
    {
        beginPos = beginPoint.position;

        xAngleTemp = xAngle;
        yAngleTemp = yAngle;

        dragging = true;

    }

    public void OnDrag(PointerEventData draggingPoint)
    {
        draggingPos = draggingPoint.position;
        yAngle = yAngleTemp + (draggingPos.x - beginPos.x) * 180 / Screen.width * rotationSpeed;
        xAngle = xAngleTemp - (draggingPos.y - beginPos.y) * 90 / Screen.height * rotationSpeed;

        player.rotation = Quaternion.Euler(xAngle, yAngle, 0.0f);
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
                Debug.Log(hit.collider.tag);
                hit.transform.GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    

    
}
