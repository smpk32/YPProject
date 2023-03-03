using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMove2 : MonoBehaviour
{
    // 플레이어 생성장소 리스트
    public List<GameObject> spawnList;

    // 맵 리스트
    public List<GameObject> mapList;


    // Start is called before the first frame update
    void Start()
    {
        SetMapList();
    }

    void SetMapList()
    {
        GameObject MapGrp = GameObject.Find("Map");

        for(int i = 0; i< MapGrp.transform.childCount; i++)
        {
            mapList.Add(MapGrp.transform.GetChild(i).gameObject);
        }

        GameObject SpawnSpotGrp = GameObject.Find("SpawnSpot");

        for (int i = 0; i < SpawnSpotGrp.transform.childCount; i++)
        {
            spawnList.Add(SpawnSpotGrp.transform.GetChild(i).gameObject);
        }
        MapChange(GameManager.instance.placeState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MapChange(int num)
    {
        GameManager.instance.placeState = num;
        
        if(GameManager.instance.multiState == "Multi" && num == 3)                  // 강당 외의 장소에서 강당으로 이동 후 placeState를 0으로 세팅해줘야 SpawnSpot위치가 맞게 설정됨
        {
            GameManager.instance.placeState = 0;
            
        }
        else if(GameManager.instance.multiState == "Multi" && num != 3)              // 강당에서 다른 장소로 이동할 때
        {
            /*Debug.Log("강당에서 다른 장소로 이동할 때");
            Debug.Log("Multi : Multi  &  num : !3");*/
            GameManager.instance.multiState = "Single";
            GameManager.instance.Disconnect();
        }
        else if(GameManager.instance.multiState == "Single" && num == 3)       // 강당 외의 장소에서 강당으로 이동할 때
        {
            /*Debug.Log("강당 외의 장소에서 강당으로 이동할 때");
            Debug.Log("Multi : Single  &  num : 3");*/
            GameManager.instance.multiState = "Multi";
            GameObject.Find("MainCanvas").gameObject.transform.Find("LoadingImage").gameObject.SetActive(true);
            GameObject.Find("MainSceneManager").GetComponent<MainSceneManager>().Enter();

        }
        else if (GameManager.instance.multiState == "Single" && num != 3)      // 강당 외의 장소에서 강당 외의 장소로 이동할 때
        {
            /*Debug.Log("강당 외의 장소에서 강당 외의 장소로 이동할 때");
            Debug.Log("Multi : Single  &  num : !3");*/
            GameManager.instance.playerPrefab = GameObject.Find("PlayerObj");
            GameManager.instance.playerPrefab.transform.parent = null;
            GameManager.instance.playerPrefab.transform.Find("Player").GetComponent<PlayerController>().Sit(false, new Vector3(0, 0, 0), new Vector3(0, 0, 0), null);
            

            for (int i = 0; i < mapList.Count; i++)
            {
                mapList[i].SetActive(false);
            }

            GameObject.Find("MainCanvas").transform.Find("FrameDtlPanel").gameObject.SetActive(false);
            GameObject.Find("MenuImage").GetComponent<MenuEvent>().HideMenuPanel();
            GameObject.Find("DragPanel").transform.Find("SitupBtn").gameObject.SetActive(false);
            mapList[num].SetActive(true);

            if (mapList[num].name == "AuditoriumGrp")
            {
                //string urlHead = "http://192.168.1.142:8060/resources/unity/StreamingAssets/";
                string urlHead = "http://192.168.1.142:8080/files/";

                // 파일서버에서 영상 불러와 재생
                //mapList[num].transform.Find("FrameGrp").GetComponent<VideoCtrl>().LoadVideo(urlHead + "yangpyeongAD.mp4");

                // 프로젝트 내부 영상 불러와 재생
                mapList[num].transform.Find("FrameGrp").GetComponent<VideoCtrl>().LoadVideo2();
            }

            SetPlayerPos(num);

        }

    }

    public void SetPlayerPos(int num)
    {
        Transform playerPos = GameManager.instance.playerPrefab.transform;
        //playerPos = GameObject.Find("PlayerObj").GetComponent<Transform>();
        playerPos.position = spawnList[num].transform.position;

        playerPos.rotation = spawnList[num].transform.rotation;
        playerPos.Find("Player").rotation = spawnList[num].transform.rotation;
        playerPos.Find("CameraObj").transform.rotation = spawnList[num].transform.rotation;
        if(GameManager.instance.multiState == "Single")
        {
            GameObject.Find("DragPanel").GetComponent<CameraRotateController>().Init();
        }
    }
}
