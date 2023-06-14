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

        if (GameManager.instance.nickNm == "������")
        {

        }
        //string urlHead = "http://192.168.1.113:8080/files/";

        // ������Ʈ ���� ���� �ҷ��� ���
        //mapList[num].transform.Find("FrameGrp").GetComponent<VideoCtrl>().LoadVideo(urlHead + "yangpyeongAD.mp4");


        // ���ϼ������� ���� �ҷ��� ���
        GameObject.Find("FrameGrp").GetComponent<VideoCtrl>().LoadVideo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
