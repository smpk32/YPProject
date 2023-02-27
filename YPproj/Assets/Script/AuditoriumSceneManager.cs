using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditoriumSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.playerPrefab == null)
        {
            GameManager.instance.CreatePlayer();
        }
        GameManager.instance.multiState = "Multi";
        GameManager.instance.playerPrefab.transform.position = GameObject.Find("spawn3").transform.position;

        //mapList[num].transform.Find("FrameGrp").GetComponent<VideoCtrl>().LoadVideo2();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
