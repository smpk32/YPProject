using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{

    public string setPlace;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // 본관 / 별관 구분자
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {


            if (setPlace == "Sub")
            {
                GameManager.Instance.bdnm = "Sub";
            }
            else if (setPlace == "Main")
            {
                GameManager.Instance.bdnm = "Main";
            }

            if (gameObject.tag == "InvPlane")
            {

                GameManager.instance.playerPrefab.transform.position = GameObject.Find("SpawnSpot").transform.Find("spawn2").transform.position;
                GameManager.instance.playerPrefab.transform.Find("Player").rotation = Quaternion.Euler(0, 180, 0);
                GameManager.instance.playerPrefab.transform.Find("CameraObj").rotation = Quaternion.Euler(0, 180, 0);

                GameObject.Find("MainCanvas").transform.Find("DragPanel").GetComponent<CameraRotateController>().Init();
            }

        }

       
    }
}
