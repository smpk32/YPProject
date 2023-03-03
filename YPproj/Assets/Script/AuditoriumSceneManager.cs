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
        //string urlHead = "http://192.168.1.142:8080/files/";

        // 파일서버에서 영상 불러와 재생
        //mapList[num].transform.Find("FrameGrp").GetComponent<VideoCtrl>().LoadVideo(urlHead + "yangpyeongAD.mp4");

        // 프로젝트 내부 영상 불러와 재생
        GameObject.Find("FrameGrp").GetComponent<VideoCtrl>().LoadVideo2();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
