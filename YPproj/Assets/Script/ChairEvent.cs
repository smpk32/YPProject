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
            if (!sitState)
            {
                sitBtn.SetActive(true);
            }
            else
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
        if (GameManager.instance.multiState == "Multi")
        {
            photonView.RPC("ChangeSitState", RpcTarget.AllBuffered, true);

        }
        //GameManager.instance.playerPrefab.transform.parent = gameObject.transform;

        if (GameManager.instance.multiState == "Single")
        {
            GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PlayerController>().Sit(true, gameObject.transform.position, gameObject.transform.rotation.eulerAngles, gameObject.name);

        }

        if(GameManager.instance.multiState == "Multi")
        {
            //pc.playerState = PlayerState.sitting;
            GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PlayerController>().SitEvent(true, gameObject.transform.position, gameObject.transform.rotation.eulerAngles, gameObject.name);
        }

        sitState = true;
        pc.playerState = PlayerState.sitting;

    }

    public void SitUp()
    {
        

        situpBtn.SetActive(false);
        if (GameManager.instance.multiState == "Multi")
        {
            photonView.RPC("ChangeSitState", RpcTarget.AllBuffered, false);

        }
        //GameManager.instance.playerPrefab.transform.parent = null;

        if (GameManager.instance.multiState == "Single")
        {
            GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PlayerController>().Sit(false, gameObject.transform.position, gameObject.transform.rotation.eulerAngles, gameObject.name);
        }

        if (GameManager.instance.multiState == "Multi")
        {
            
            GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PlayerController>().SitEvent(false, gameObject.transform.position, gameObject.transform.rotation.eulerAngles, gameObject.name);
        }

        sitState = false;
        pc.playerState = PlayerState.normal;
        //GameManager.instance.playerPrefab.transform.parent.GetComponent<ChairEvent>().SitPlayer();
    }

    public void SitClickEvent()
    {
        Action _action = () => SitDown();
        GameManager.instance.playerPrefab.GetComponent<PlayerNav>().MovingToTarget(gameObject, _action);
    }



    [PunRPC]
    public void ChangeSitState(bool isSit)
    {
        sitState = isSit;
        sitBtn.SetActive(false);

        Debug.Log(sitState);
    }

}
