using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairEvent : MonoBehaviourPun
{

    Transform target;

    // ���ڿ� �ɱⰡ�� üũ �Լ�
    public bool sitState;

    // ��ü ���� ����Ʈ
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
            pc = GameObject.Find("Player")?.GetComponent<PlayerController>();
            return;
        }

        if (target != null && GameManager.instance.playerState == PlayerState.normal)
        {
            if (!sitState && !sitBtn.activeSelf)
            {
                sitBtn.SetActive(true);
            }
            else if (sitState && !sitBtn.activeSelf)
            {
                
                // SitObj�� �ڽĿ�����Ʈ�� ���� �� ������ null�� �ޱ� ���� ? ���    * ?�� ������� ������ ���� ���
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

    // �ɱ� �Լ�
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

            Vector3 chairPos = gameObject.transform.position;
            chairPos.y = chairPos.y - GameManager.instance.playerPrefab.GetComponent<PlayerModelInfo>().modelSitHeight;


            GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PlayerController>().Sit(true, chairPos, gameObject.transform.rotation.eulerAngles, gameObject.name);

        }

        if(GameManager.instance.multiState == "Multi")
        {
            //pc.playerState = PlayerState.sitting;
            //photonView.RPC("ChangeSitState", RpcTarget.AllBuffered, true);
            GameManager.instance.sitNm = gameObject.name;

            Vector3 chairPos = gameObject.transform.position;

            chairPos.y = chairPos.y - GameManager.instance.playerPrefab.GetComponent<PlayerModelInfo>().modelSitHeight;

            GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PlayerController>().SitEvent(true, chairPos, gameObject.transform.rotation.eulerAngles);
            GameManager.instance.playerPrefab.transform.Find("CameraObj").transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles + new Vector3(15, 0, 0));
            GameObject.Find("DragPanel").GetComponent<CameraRotateController>().Init();
        }

        sitState = true;
        GameManager.instance.playerState = PlayerState.sitting;

    }

    // �Ͼ�� �Լ�
    public void SitUp()
    {
        

        situpBtn.SetActive(false);


        if (GameManager.instance.multiState == "Single")
        {
            GameObject chair = GameObject.Find(GameManager.instance.sitNm);
            GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PlayerController>().Sit(false, chair.transform.position, chair.transform.rotation.eulerAngles, GameManager.instance.sitNm);
        }

        // ��Ƽ�÷����϶� PUNRPC�Լ� ����
        if (GameManager.instance.multiState == "Multi")
        {
            photonView.RPC("ChangeSitState", RpcTarget.AllBuffered, GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PhotonView>().ViewID, false, GameManager.instance.sitNm);


            //GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PlayerController>().SitEvent(false, gameObject.transform.position, gameObject.transform.rotation.eulerAngles);

            GameObject chair = GameObject.Find(GameManager.instance.sitNm);
            GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PlayerController>().SitEvent(false, chair.transform.position, chair.transform.rotation.eulerAngles);
            GameManager.instance.sitNm = null;
        }
        GameObject.Find("DragPanel").GetComponent<CameraRotateController>().Init();
        sitState = false;
        GameManager.instance.playerState = PlayerState.normal;

    }

    // ���� �ɱ� Ŭ���̺�Ʈ �Լ�
    public void SitClickEvent()
    {

        if(GameManager.instance.playerState == PlayerState.chat)
        {
            return;
        }

        Action _action = () => SitDown();
        GameManager.instance.playerPrefab.GetComponent<PlayerNav>().MovingToTarget(gameObject, _action);
        GameManager.instance.sitNm = gameObject.name;
        if (GameManager.instance.multiState == "Multi")
        {
            photonView.RPC("ChangeSitState", RpcTarget.AllBuffered, GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PhotonView>().ViewID, true, gameObject.name);
        }
    }


    // �ٸ� �÷��̾�� ���ڻ��� ���� �˸��� ���� �Լ�
    [PunRPC]
    public void ChangeSitState(int viewID,bool isSit, string chairNm)
    {
        sitState = isSit;
        sitBtn.SetActive(false);


        // �ɱ� Ŭ�����ڸ��� �ٸ� �÷��̾ �ش� �ڸ��� �� �ɰ� �ϱ� ���� ChangeSitState���� �Ʒ� �ڵ� ����
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
