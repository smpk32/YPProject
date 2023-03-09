using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMove2 : MonoBehaviour
{
    // �÷��̾� ������� ����Ʈ
    public List<GameObject> spawnList;

    // �� ����Ʈ
    public List<GameObject> mapList;


    // Start is called before the first frame update
    void Start()
    {
        SetMapList();
    }


    // �ʱ� �� ����Ʈ ���� �Լ�
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


    /* �� �̵� �̺�Ʈ �Լ�
     * 0 > ���ð� 1
     * 1 > ���ð� 2
     * 2 > �����Ѹ��� (�̱�)
     * 3 > �����Ѹ��� (��Ƽ)
     */
    public void MapChange(int num)
    {
        GameManager.instance.placeState = num;
        
        if (GameManager.instance.multiState == "Multi" && num == 3)                  // ���� ���� ��ҿ��� �������� �̵� �� placeState�� 0���� ��������� SpawnSpot��ġ�� �°� ������
        {
            GameManager.instance.placeState = 0;
            
        }
        else if(GameManager.instance.multiState == "Multi" && num != 3)              // ���翡�� �ٸ� ��ҷ� �̵��� ��
        {
            /*Debug.Log("���翡�� �ٸ� ��ҷ� �̵��� ��");
            Debug.Log("Multi : Multi  &  num : !3");*/
            GameManager.instance.multiState = "Single";
            GameManager.instance.Disconnect();
        }
        else if(GameManager.instance.multiState == "Single" && num == 3)       // ���� ���� ��ҿ��� �������� �̵��� ��
        {
            /*Debug.Log("���� ���� ��ҿ��� �������� �̵��� ��");
            Debug.Log("Multi : Single  &  num : 3");*/
            GameManager.instance.multiState = "Multi";
            GameObject.Find("MainCanvas").gameObject.transform.Find("LoadingImage").gameObject.SetActive(true);
            GameObject.Find("MainSceneManager").GetComponent<MainSceneManager>().Enter();

        }
        else if (GameManager.instance.multiState == "Single" && num != 3)      // ���� ���� ��ҿ��� ���� ���� ��ҷ� �̵��� ��
        {
            /*Debug.Log("���� ���� ��ҿ��� ���� ���� ��ҷ� �̵��� ��");
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

                // ���ϼ������� ���� �ҷ��� ���
                //mapList[num].transform.Find("FrameGrp").GetComponent<VideoCtrl>().LoadVideo(urlHead + "yangpyeongAD.mp4");

                // ������Ʈ ���� ���� �ҷ��� ���
                mapList[num].transform.Find("FrameGrp").GetComponent<VideoCtrl>().LoadVideo2();
            }
            else
            {
                mapList[num].transform.Find("FrameGrp").GetComponent<FrameSet>().FloorChange(0);
            }

            SetPlayerPos(num);

        }

    }

    // �÷��̾� ����, ��� �̵� �� �÷��̾� ĳ����, ī�޶� ����
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
