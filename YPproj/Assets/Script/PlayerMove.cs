using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{

    RaycastHit hit;
    Ray ray;

#if UNITY_WEBGL && !UNITY_EDITOR
    // 플레이어 이동 속도 값
    float moveSpeed = 5.0f;
#elif PLATFORM_STANDALONE_WIN && !UNITY_EDITOR
    float moveSpeed = 5.0f;
#else
    float moveSpeed = 30.0f;
#endif
    // 플레이어 점프 값 
    float jumpPower = 3.0f;
    Rigidbody rd;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        //Rotate();



    }


    // 플레이어 이동 이벤트
    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //transform.Translate((new Vector3(h, 0, v) * moveSpeed) * Time.deltaTime);

        Vector3 moveDistance = v * transform.forward * moveSpeed * Time.deltaTime;

        Vector3 LRDistance = h * transform.right * moveSpeed * Time.deltaTime;

        rd.MovePosition(transform.position+ moveDistance+ LRDistance);


        //Translate 함수를 사용한 이동 로직
        /*tr.Translate(Vector3.forward * Time.deltaTime * v * moveSpeed);

        tr.Translate(Vector3.right * Time.deltaTime * h * moveSpeed);*/
    }

    // 플레이어 회전 이벤트
    void Rotate()
    {
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0, 50f * Time.deltaTime, 0));
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, -(50f * Time.deltaTime), 0));
        }
    }

    // 플레이어 점프 이벤트
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rd.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    void PlayerClick()
    {
        //마우스 왼쪽클릭시
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "EvtBtn")
                {
                    Debug.Log("EvtBtn");
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                }
            }
        }
    }
}
