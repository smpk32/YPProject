using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void SelectFloor(string selectVal)
    {
        GameObject panel = GameObject.Find("MainCanvas").transform.Find("PortalPanel").transform.gameObject;
        panel.SetActive(false);
/*
        if()
        GameManager.instance.playerPrefab.transform = GameObject.Find("MoveSpot").t
*/
        GameManager.instance.playerState = PlayerState.normal;

    }
}
