using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gravitons.UI.Modal;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public string floorSt;
    public int floor;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            if (floorSt == "bri")
            {
                GameObject potal = GameObject.Find("Portal").transform.Find("BridgePortal").gameObject;
                string floor = GameManager.Instance.floor.ToString();
                
                if (GameManager.Instance.bdnm == "Main")
                {
                    GameManager.instance.playerPrefab.transform.position = potal.transform.Find("SubSpot").transform.Find(floor).transform.position;
                    GameManager.instance.bdnm = "Sub";

                    //GameObject.Find("MainCanvas").transform.Find("DragPanel").GetComponent<CameraRotateController>().Init();
                    //GameManager.instance.playerPrefab.transform.Find("Player").transform.Rotate(0,90,0);

                }
                else
                {
                    GameManager.instance.playerPrefab.transform.position = potal.transform.Find("MainSpot").transform.Find(floor).transform.position;
                    GameManager.instance.bdnm = "Main";

                    //GameObject.Find("MainCanvas").transform.Find("DragPanel").GetComponent<CameraRotateController>().Init();
                    //GameManager.instance.playerPrefab.transform.Find("Player").transform.Rotate(0, 90, 0);

                }
            }
            else
            {

            GameObject.Find("MainCanvas").transform.Find("PortalPanel").transform.gameObject.SetActive(true);

            GameManager.instance.playerState = PlayerState.setting;
            }


        }
    }
  
}
