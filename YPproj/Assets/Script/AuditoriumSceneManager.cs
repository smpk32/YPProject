using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditoriumSceneManager : MonoBehaviour
{

    private void Awake()
    {
        GameObject.Find("MainCanvas").gameObject.transform.Find("LoadingImage").gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.playerPrefab = null;
        /*if (GameManager.instance.playerPrefab == null)
        {
            GameManager.instance.CreatePlayer();
        }*/

        if (GameManager.instance.nickNm == "군수님")
        {

        }
        //string urlHead = "http://192.168.1.113:8080/files/";

        // 프로젝트 내부 영상 불러와 재생
        //mapList[num].transform.Find("FrameGrp").GetComponent<VideoCtrl>().LoadVideo(urlHead + "yangpyeongAD.mp4");


        // 파일서버에서 영상 불러와 재생
        GameObject.Find("FrameGrp").GetComponent<VideoCtrl>().LoadVideo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
