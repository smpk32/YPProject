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

                    GameManager.instance.playerPrefab.transform.Find("Player").rotation = Quaternion.Euler(0, 180, 0);
                    GameManager.instance.playerPrefab.transform.Find("CameraObj").rotation = Quaternion.Euler(0, 180, 0);

                    GameObject.Find("MainCanvas").transform.Find("DragPanel").GetComponent<CameraRotateController>().Init();

                }
                else
                {
                    GameManager.instance.playerPrefab.transform.position = potal.transform.Find("MainSpot").transform.Find(floor).transform.position;
                    GameManager.instance.bdnm = "Main";

                    GameManager.instance.playerPrefab.transform.Find("Player").rotation = Quaternion.Euler(0, -90, 0);
                    GameManager.instance.playerPrefab.transform.Find("CameraObj").rotation = Quaternion.Euler(0, -90, 0);

                    GameObject.Find("MainCanvas").transform.Find("DragPanel").GetComponent<CameraRotateController>().Init();

                }
            }
            else
            {
                GameObject.Find("MainCanvas").transform.Find("PortalPanel").transform.gameObject.SetActive(true);
                GameManager.instance.playerState = PlayerState.setting;

                if (GameManager.Instance.bdnm == "Main")
                {
                    GameObject.Find("MainCanvas").transform.Find("PortalPanel").transform.Find("TabBox1").transform.gameObject.SetActive(true);
                    GameObject.Find("MainCanvas").transform.Find("PortalPanel").transform.Find("TabBox2").transform.gameObject.SetActive(false);
                }
                else
                {
                    GameObject.Find("MainCanvas").transform.Find("PortalPanel").transform.Find("TabBox1").transform.gameObject.SetActive(false);
                    GameObject.Find("MainCanvas").transform.Find("PortalPanel").transform.Find("TabBox2").transform.gameObject.SetActive(true);
                }


            }


        }
    }
  
}
