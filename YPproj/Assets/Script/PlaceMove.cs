using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMove : MonoBehaviour
{

    // 플레이어 생성장소 리스트
    public List<GameObject> spawnList;

    // 맵 리스트
    public List<GameObject> mapList;

    // 장소이동 패널
    GameObject placeMovePanel;

    Transform playerPos;

    //int listCnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("PlayerObj").GetComponent<Transform>();
        placeMovePanel = GameObject.Find("MainCanvas").transform.Find("MenuPanelGrp").transform.Find("PlaceMovePanel").gameObject;
    }

    public void MapChange(int num)
    {
        /*listCnt++;

        if(listCnt >= spawnList.Count)
        {
            listCnt = 0;
        }*/
        GameObject.Find("PlayerObj").transform.parent = null;
        GameObject.Find("Player").GetComponent<PlayerController>().Sit(false, new Vector3(0,0,0), new Vector3(0, 0, 0));
        for (int i = 0; i<mapList.Count; i++)
        {
            mapList[i].SetActive(false);
        }

        GameObject.Find("MainCanvas").transform.Find("FrameDtlPanel").gameObject.SetActive(false);
        GameObject.Find("MenuImage").GetComponent<MenuEvent>().HideMenuPanel();
        mapList[num].SetActive(true);
        if(mapList[num].name == "AuditoriumGrp")
        {
            //string urlHead = "http://192.168.1.142:8060/resources/unity/StreamingAssets/";
            string urlHead = "http://192.168.1.142:8080/files/";

            mapList[num].transform.Find("FrameGrp").GetComponent<VideoCtrl>().LoadVideo(urlHead + "yangpyeongAD.mp4");
        }
        playerPos.position = spawnList[num].transform.position;

        playerPos.rotation = spawnList[num].transform.rotation;
        GameObject.Find("Player").transform.rotation = spawnList[num].transform.rotation;
        GameObject.Find("CameraObj").transform.rotation = spawnList[num].transform.rotation;
        GameObject.Find("DragPanel").GetComponent<CameraRotateController>().Init();

    }

    
}
