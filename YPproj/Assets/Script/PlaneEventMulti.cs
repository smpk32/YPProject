using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneEventMulti : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            if (gameObject.tag == "InvPlane")
            {
                GameManager.instance.playerPrefab.transform.position = GameObject.Find("SpawnSpot").transform.Find("spawn1").transform.position;
                GameManager.instance.playerPrefab.transform.Find("Player").rotation = Quaternion.Euler(0, 180, 0);
                GameManager.instance.playerPrefab.transform.Find("CameraObj").rotation = Quaternion.Euler(0, 180, 0);

                GameObject.Find("MainCanvas").transform.Find("DragPanel").GetComponent<CameraRotateController>().Init();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            if (gameObject.tag == "InvPlane")
            {
                GameManager.instance.playerPrefab.transform.position = GameObject.Find("SpawnSpot").transform.Find("spawn1").transform.position;
                GameManager.instance.playerPrefab.transform.Find("Player").rotation = Quaternion.Euler(0, 180, 0);
                GameManager.instance.playerPrefab.transform.Find("CameraObj").rotation = Quaternion.Euler(0, 180, 0);

                GameObject.Find("MainCanvas").transform.Find("DragPanel").GetComponent<CameraRotateController>().Init();
            }
        }

    }
}
