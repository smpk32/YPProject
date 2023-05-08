using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPanel : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void SelectFloor(int selectVal)
    {
        GameObject panel = GameObject.Find("MainCanvas").transform.Find("PortalPanel").transform.gameObject;
        panel.SetActive(false);
        if (GameManager.Instance.bdnm == "Main")
        {
            GameManager.instance.playerPrefab.transform.position = GameObject.Find("Portal").transform.Find("ElePortal").transform.Find("MainMoveSpot").Find(selectVal.ToString()).transform.position;

        }
        else
        {
            GameManager.instance.playerPrefab.transform.position = GameObject.Find("Portal").transform.Find("ElePortal").transform.Find("SubMoveSpot").Find(selectVal.ToString()).transform.position;
        }
       
        GameManager.instance.playerState = PlayerState.normal;

    }

    public void ClosePortalPanel()
    {
        gameObject.SetActive(false);

        GameManager.instance.playerState = PlayerState.normal;
    }
}
