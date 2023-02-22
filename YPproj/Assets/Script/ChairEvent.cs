using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairEvent : MonoBehaviour
{

    Transform target;

    bool sitState;

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
        pc = GameObject.Find("Player").GetComponent<PlayerController>();


        InvokeRepeating("UpdateTarget", 0f, 0.25f);

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && pc.playerState == PlayerState.normal)
        {
            sitBtn.SetActive(true);
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

    public void SitPlayer()
    {
        sitState = !sitState;
        
        if (sitState)
        {
            for(int i = 0; i < chairList.Length; i++)
            {
                chairList[i].gameObject.transform.Find("SitPanel").gameObject.SetActive(false);
            }

            situpBtn.SetActive(true);
            sitBtn.SetActive(false);
            GameObject.Find("PlayerObj").transform.parent = gameObject.transform;
            pc.Sit(sitState, gameObject.transform.position, gameObject.transform.rotation.eulerAngles);
        }
        else
        {
            situpBtn.SetActive(false);
            GameObject.Find("PlayerObj").transform.parent = null;
            pc.Sit(sitState, gameObject.transform.position, gameObject.transform.rotation.eulerAngles);
        }
            

    }

    public void SitUp()
    {
        GameObject.Find("PlayerObj").transform.parent.GetComponent<ChairEvent>().SitPlayer();
    }

    public void SitClickEvent()
    {
        Action _action = () => SitPlayer();
        GameObject.Find("PlayerObj").GetComponent<PlayerNav>().MovingToTarget(gameObject, _action);
    }


}
