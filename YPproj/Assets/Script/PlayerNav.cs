using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNav : MonoBehaviour
{

    private NavMeshAgent agent = null;
    private Animator animator;

    GameObject playerobj;
    GameObject playerModel;
    private Rigidbody rigidBody;
    bool checkBtn;

    Action action;


    // Start is called before the first frame update
    void Start()
    {
        playerobj = GameObject.Find("PlayerObj").transform.gameObject; //player obj
        playerModel = playerobj.transform.Find("Player").gameObject;
        agent = playerobj.GetComponent<NavMeshAgent>(); //player nav mesh agent 
        rigidBody = playerobj.GetComponent<Rigidbody>();// rigid body
        animator = playerobj.transform.Find("Player").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.sqrMagnitude >= 0.2f && agent.remainingDistance <= 0.3f)
        {
            StopMovingToTgarget();
            action();
        }
        if (checkBtn)
        {
            Vector3 dir = agent.steeringTarget - playerobj.transform.position; 
            dir.y = 0;

            playerModel.transform.rotation = Quaternion.LookRotation(dir.normalized);




        }
    }
    

    public void MovingToTarget(GameObject _targetObj, Action _action)
    {
        agent.speed = 7f;

        checkBtn = true;
        agent.enabled = true;
        rigidBody.isKinematic = true;
        animator.SetBool("Run", true);
        //playerModel.GetComponent<PlayerController>().playerState = PlayerState.nav;

        //targetObj = _targetObj;
        action = _action;
        //playerobj.transform.rotation = playerModel.transform.rotation;
        agent.SetDestination(_targetObj.transform.position);
        

        //playerobj.transform.Find("CameraObj").transform.rotation = Quaternion.Euler(playerobj.transform.rotation.eulerAngles+ new Vector3(15, 0, 0));
        
        //GameObject.Find("DragPanel").GetComponent<CameraRotateController>().Init();

    }


    void StopMovingToTgarget()
    {

        agent.speed = 0f;
        checkBtn = false;
        agent.enabled = false;
        rigidBody.isKinematic = false;
        animator.SetBool("Run", false);

        //자동이동 시 플레이어 카메라 위치를 플레이어 뒤로 설정하는 코드  *2023.02.22 주석처리
        /*Quaternion playerRot = playerobj.transform.rotation;
        playerobj.transform.rotation = Quaternion.Euler( new Vector3(0, 0, 0));
        playerModel.transform.rotation = playerRot;
        playerobj.transform.Find("CameraObj").transform.rotation = Quaternion.Euler(playerRot.eulerAngles + new Vector3(15, 0, 0));
        GameObject.Find("DragPanel").GetComponent<CameraRotateController>().Init();*/

    }
}
