using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairEvent : MonoBehaviourPun
{

    Transform target;

    public bool sitState;

    ChairEvent[] chairList;
    GameObject sitBtn;
    GameObject situpBtn;

    PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        sitBtn = gameObject.transform.Find("SitPanel").gameObject;
        situpBtn = GameObject.Find("DragPanel").transform.Find("SitupBtn").gameObject;
        chairList = transform.parent.GetComponentsInChildren<ChairEvent>();
        //pc = GameObject.Find("Player").GetComponent<PlayerController>();




        InvokeRepeating("UpdateTarget", 0f, 0.25f);

    }

    // Update is called once per frame
    void Update()
    {

        if(pc == null)
        {
            pc = GameObject.Find("Player").GetComponent<PlayerController>();
            return;
        }

        if (target != null && pc.playerState == PlayerState.normal)
        {
            if (!sitState && !sitBtn.activeSelf)
            {
                sitBtn.SetActive(true);
            }
            else if (sitState && !sitBtn.activeSelf)
            {
                
                // SitObj가 자식오브젝트에 없을 때 리턴을 null로 받기 위해 ? 사용    * ?를 사용하지 않으면 에러 출력
                if (gameObject.transform.Find("SitObj")?.gameObject == null)
                {
                    sitBtn.SetActive(true);
                    sitState = false;
                }
            }
            

        }



    }

    private void UpdateTarget()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 20f, 1 << 6);

        if (cols.Length > 0)
        {

            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].tag == "Player")
                {
                    target = cols[i].gameObject.transform;
                }
            }
        }
        else
        {
            if (sitBtn.activeSelf)
            {
                sitBtn.SetActive(false);
            }
            target = null;
        }

    }

    public void SitDown()
    {
        
        

        for(int i = 0; i < chairList.Length; i++)
        {
            chairList[i].gameObject.transform.Find("SitPanel").gameObject.SetActive(false);
        }

        situpBtn.SetActive(true);
        sitBtn.SetActive(false);

        //GameManager.instance.playerPrefab.transform.parent = gameObject.transform;

        if (GameManager.instance.multiState == "Single")
        {
            GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PlayerController>().Sit(true, gameObject.transform.position, gameObject.transform.rotation.eulerAngles, gameObject.name);

        }

        if(GameManager.instance.multiState == "Multi")
        {
            //pc.playerState = PlayerState.sitting;
            //photonView.RPC("ChangeSitState", RpcTarget.AllBuffered, true);
            GameManager.instance.sitNm = gameObject.name;
            GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PlayerController>().SitEvent(true, gameObject.transform.position, gameObject.transform.rotation.eulerAngles);
            GameManager.instance.playerPrefab.transform.Find("CameraObj").transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles + new Vector3(15, 0, 0));
            GameObject.Find("DragPanel").GetComponent<CameraRotateController>().Init();
        }

        sitState = true;
        pc.playerState = PlayerState.sitting;

    }

    public void SitUp()
    {
        

        situpBtn.SetActive(false);
        
        //GameManager.instance.playerPrefab.transform.parent = null;

        if (GameManager.instance.multiState == "Single")
        {
            GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PlayerController>().Sit(false, gameObject.transform.position, gameObject.transform.rotation.eulerAngles, gameObject.name);
        }

        if (GameManager.instance.multiState == "Multi")
        {
            photonView.RPC("ChangeSitState", RpcTarget.AllBuffered, GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PhotonView>().ViewID, false, GameManager.instance.sitNm);
            GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PlayerController>().SitEvent(false, gameObject.transform.position, gameObject.transform.rotation.eulerAngles);
            GameManager.instance.sitNm = null;
        }

        sitState = false;
        pc.playerState = PlayerState.normal;
        //GameManager.instance.playerPrefab.transform.parent.GetComponent<ChairEvent>().SitPlayer();
    }

    public void SitClickEvent()
    {
        Action _action = () => SitDown();
        GameManager.instance.playerPrefab.GetComponent<PlayerNav>().MovingToTarget(gameObject, _action);
        GameManager.instance.sitNm = gameObject.name;
        if (GameManager.instance.multiState == "Multi")
        {
            photonView.RPC("ChangeSitState", RpcTarget.AllBuffered, GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PhotonView>().ViewID, true, gameObject.name);
        }
    }



    [PunRPC]
    public void ChangeSitState(int viewID,bool isSit, string chairNm)
    {
        sitState = isSit;
        sitBtn.SetActive(false);


        // 앉기 클릭하자마자 다른 플레이어가 해당 자리에 못 앉게 하기 위해 ChangeSitState에서 아래 코드 실행
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");



        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PhotonView>().ViewID == viewID)
            {
                if (isSit)
                {
                    if (chairNm != null)
                    {
                        players[i].transform.parent.transform.Find("SitObj").parent = GameObject.Find(chairNm).transform;
                        //GameManager.instance.sitNm = chairNm;
                    }
                }
                else
                {
                    if (chairNm != null)
                    {
                        GameObject.Find(chairNm).transform.Find("SitObj").transform.parent = players[i].transform.parent;
                    }
                }
            }

        }


    }

}
