using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using static InhbtntInfo;

public class PlaceMove : MonoBehaviour
{
    // �����������Ʈ
    [Serializable]
    class EventListData
    {
        public string event_id;                 // ��� id
        public string event_nm;                 // ��� ��
        public string event_image_atfl_id;      // ��� ������ �̹��� ���� id
        public string event_dc;                 // ��� ����
        public string event_bgng_dt;            // ��� ���� ����
        public string event_end_dt;             // ��� ���� ����
        public string event_place;              // ��� ���
        public string event_hmpg_url;           // ��� ������ url
        public string progress;                 // ��� ����

    }

    //�ֹμ���ȸ ���� ����Ʈ
    class InhbtntListData
    {
        public string inhbtnt_pran_id;              // �ֹμ���ȸ id
        public string inhbtnt_pran_nm;              // �ֹμ���ȸ ��
        public string inhbtnt_pran_dc;              // �ֹμ���ȸ ����
        public string inhbtnt_pran_bgng_dt;         // �ֹμ���ȸ ������
        public string inhbtnt_pran_end_dt;          // �ֹμ���ȸ ������
        public string inhbtnt_pran_atfl_id;         // �ֹμ���ȸ ���� url
        public string use_yn;                       // �ֹμ���ȸ ǥ�� ����
        public string progress;
        public string stre_file_nm;

    }


    // �÷��̾� ������� ����Ʈ
    public List<GameObject> spawnList;

    // �� ����Ʈ
    public List<GameObject> mapList;

    // ������� ����Ʈ ������Ʈ
    GameObject eventListObj;
    // �ֹμ���ȸ ����Ʈ ������Ʈ
    GameObject InhbtntListObj;

    public Sprite postImg;

    void Awake()
    {
        eventListObj = Resources.Load<GameObject>("EventList\\EventListObj");
        InhbtntListObj = Resources.Load<GameObject>("Prefabs\\InhbtntBnt");
    }
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
        //MapChange(GameManager.instance.placeState);
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
        GameManager.instance.SetState("normal");
        GameManager.instance.placeState = num;
        
        if (GameManager.instance.multiState == "Multi" && num == 3)                  // ���� ���� ��ҿ��� �������� �̵� �� placeState�� 0���� ��������� SpawnSpot��ġ�� �°� ������
        {
            
            GameManager.instance.placeState = 0;
            SetPlayerPos(GameManager.instance.placeState);

        }
        else if(GameManager.instance.multiState == "Multi" && num != 3)              // ���翡�� �ٸ� ��ҷ� �̵��� ��
        {
            GameObject.Find("MainCanvas").gameObject.transform.Find("LoadingImage").gameObject.SetActive(true);
            GameObject.Find("MainCanvas").GetComponent<Chat>().CloseSocket();
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

            // ��� žĵ���� SetActive Falseó��
            if (mapList.Count != 0)
            {
                mapList[1].transform.Find("FrameGrp").transform.Find("FloorCanvas").gameObject.SetActive(false);
            }

            GameObject.Find("MainSceneManager").GetComponent<MainSceneManager>().Enter();
            
            

        }
        else if (GameManager.instance.multiState == "Single" && num != 3)      // ���� ���� ��ҿ��� ���� ���� ��ҷ� �̵��� ��
        {
            if (GameManager.instance.placeState == 2)
            {
                Camera.main.GetComponent<AudioSource>().mute = true;
            }
            else
            {
                Camera.main.GetComponent<AudioSource>().mute = false;
            }
            /*Debug.Log("���� ���� ��ҿ��� ���� ���� ��ҷ� �̵��� ��");
            Debug.Log("Multi : Single  &  num : !3");*/
            //GameManager.instance.playerPrefab = GameObject.Find("PlayerObj");
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
                //string urlHead = "http://192.168.1.113:8060/resources/unity/StreamingAssets/";
                //string urlHead = "http://192.168.1.113:8080/files/";

                // ���ϼ������� ���� �ҷ��� ���
                //mapList[num].transform.Find("FrameGrp").GetComponent<VideoCtrl>().LoadVideo(urlHead + "yangpyeongAD.mp4");

                // ������Ʈ ���� ���� �ҷ��� ���
                mapList[num].transform.Find("FrameGrp").GetComponent<VideoCtrl>().LoadVideo();
            }
            else if(mapList[num].name == "GalleryGrp (1)")
            {
                mapList[num].transform.Find("FrameGrp").GetComponent<FrameSet>().EnterGallery();
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

    public void CloseEventListPanel()
    {
        GameObject.Find("MenuPanelGrp").transform.Find("PlaceMovePanel").gameObject.SetActive(true);
        GameObject.Find("MenuPanelGrp").transform.Find("EventListPanel").gameObject.SetActive(false);
    }

    // ��� �̵� > ���ð����� �̵� �̺�Ʈ
    public void InitEventList()
    {
        StartCoroutine(SelectEventList("",(data) =>
        {
            var dataSet = JsonConvert.DeserializeObject<List<EventListData>>(data);


            GameObject.Find("MenuPanelGrp").transform.Find("PlaceMovePanel").gameObject.SetActive(false);
            GameObject.Find("MenuPanelGrp").transform.Find("EventListPanel").gameObject.SetActive(true);

            HorizontalScrollSnap scrollSnap = GameObject.Find("MenuPanelGrp").transform.Find("EventListPanel").transform.Find("Canvas").transform.Find("Mask").transform.Find("HorizontalScrollSnap").GetComponent<HorizontalScrollSnap>();

            GameObject[] Content = scrollSnap.ChildObjects;
            //����
            foreach (GameObject child in Content)
            {
                Destroy(child.gameObject);
            }

            scrollSnap.RemoveAllChildren(out scrollSnap.ChildObjects);

            for (int i = 0; i <dataSet.Count; i++)
            {

                GameObject eventList = Instantiate(eventListObj);

                eventList.GetComponent<EventInfo>().eventDtlInfo = new EventInfo.EventDtlInfo(dataSet[i].event_id, dataSet[i].event_nm, dataSet[i].event_image_atfl_id, dataSet[i].event_dc, dataSet[i].event_bgng_dt, dataSet[i].event_end_dt, dataSet[i].event_place, dataSet[i].event_hmpg_url, dataSet[i].progress);


                if (dataSet[i].progress.Equals("N"))
                {
                    eventList.transform.Find("ProgressImg").GetComponent<Image>().color = new Color(1, 0.73f, 0);
                    eventList.transform.Find("ProgressImg").transform.Find("ProgressText").GetComponent<TextMeshProUGUI>().text = "����";

                }
                // �г� ����
                StartCoroutine(LoadImageTexture(eventList.transform.Find("Poster").GetComponent<Image>(), dataSet[i].event_image_atfl_id));
                eventList.transform.Find("TitleText").GetComponent<TextMeshProUGUI>().text = dataSet[i].event_nm;
                eventList.transform.Find("DcText").GetComponent<TextMeshProUGUI>().text = dataSet[i].event_dc;
                eventList.transform.Find("DtText").GetComponent<TextMeshProUGUI>().text = dataSet[i].event_bgng_dt + " ~ " + dataSet[i].event_end_dt;
                eventList.transform.Find("PlaceText").GetComponent<TextMeshProUGUI>().text = dataSet[i].event_place;
                eventList.transform.Find("UrlText").GetComponent<TextMeshProUGUI>().text = dataSet[i].event_hmpg_url;

                scrollSnap.AddChild(eventList);

            }


        }));
    }

    public IEnumerator SelectEventList(string listType, Action<string> callback)
    {
        string GetDataUrl = GameManager.Instance.baseURL + "/event/list";

        using (UnityWebRequest www = UnityWebRequest.Post(GetDataUrl,""))
        {
            yield return www.SendWebRequest();
            // yield return System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) //�ҷ����� ���� ��
            {
              
                yield return "error";
            }
            else
            {
                if (www.isDone)
                {
                 

                    if (www.downloadHandler.data == null)
                    {

                        yield return null;

                    }
                    else
                    {
                        callback(System.Text.Encoding.UTF8.GetString(www.downloadHandler.data));
                        //yield return System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    }

                }
            }
        }

    }

    // ��� �̵� > ���ð����� �̵� �̺�Ʈ
    public void InitInhbtntList()
    {
        StartCoroutine(SelectinhbtntList("", (data) =>
        {
         
            var dataSet = JsonConvert.DeserializeObject<List<InhbtntListData>>(data);

            GameObject.Find("MenuPanelGrp").transform.Find("PlaceMovePanel").gameObject.SetActive(false);
            GameObject.Find("MenuPanelGrp").transform.Find("PresentationPanel").gameObject.SetActive(true);

            Transform Content = GameObject.Find("MenuPanelGrp").transform.Find("PresentationPanel").transform.Find("Scroll View").transform.Find("Viewport").transform.Find("Content").transform;

            //����
            foreach (Transform child in Content)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < dataSet.Count; i++)
            {
        
                GameObject InhbtntList = Instantiate(InhbtntListObj);

                InhbtntList.GetComponent<InhbtntInfo>().inhbtntdtlData = new InhbtntInfo.InhbtntData(
                    dataSet[i].inhbtnt_pran_id,
                    dataSet[i].inhbtnt_pran_nm,
                    dataSet[i].inhbtnt_pran_dc,
                    dataSet[i].inhbtnt_pran_bgng_dt,
                    dataSet[i].inhbtnt_pran_end_dt,
                    dataSet[i].inhbtnt_pran_atfl_id,
                    dataSet[i].use_yn,
                    dataSet[i].progress,
                    dataSet[i].stre_file_nm
                    );


                /*string id = dataSet[i].inhbtnt_pran_id;
                InhbtntList.GetComponent<Button>().onClick.AddListener(()=> InhbtntList.GetComponent<InhbtntInfo>().ClickInhbBtn(id));*/

                var progress = dataSet[i].progress;
                if (progress =="Y" )
                {

                    InhbtntList.transform.Find("TitleText").GetComponent<TextMeshProUGUI>().text = dataSet[i].inhbtnt_pran_nm;
                    InhbtntList.transform.Find("ProgressImg").GetComponent<Image>().color = new Color(0, 0.8f, 0);
                    InhbtntList.transform.Find("ProgressImg").transform.Find("ProgressText").GetComponent<TextMeshProUGUI>().text = "������";


                }
                else
                {
                    InhbtntList.transform.Find("TitleText").GetComponent<TextMeshProUGUI>().text = dataSet[i].inhbtnt_pran_nm;
                    InhbtntList.transform.Find("ProgressImg").GetComponent<Image>().color = new Color(1, 0.73f, 0);
                    InhbtntList.transform.Find("ProgressImg").transform.Find("ProgressText").GetComponent<TextMeshProUGUI>().text = "����";
                }

                //false ���� �θ����Ϸ� ������ �׳� ������� ���� �����Ϸ� ����~
                InhbtntList.transform.SetParent(Content, false);

            }

            


        }));
    }

    
    public IEnumerator SelectinhbtntList(string listType, Action<string> callback)
    {
        string GetDataUrl = GameManager.Instance.baseURL + "/inhbtnt/list";

        using (UnityWebRequest www = UnityWebRequest.Get(GetDataUrl))
        {
            yield return www.SendWebRequest();
            // yield return System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) //�ҷ����� ���� ��
            {
             
                yield return "error";
            }
            else
            {
                if (www.isDone)
                {
                  

                    if (www.downloadHandler.data == null)
                    {

                        yield return null;

                    }
                    else
                    {
                        callback(System.Text.Encoding.UTF8.GetString(www.downloadHandler.data));
                        //yield return System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    }

                }
            }
        }

    }


    IEnumerator LoadImageTexture(Image rawImg, string fileId)
    {

        //UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://192.168.1.113:8080/selectImg?file_nm="+ fileId);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(GameManager.instance.baseURL + "/display?filename=" + fileId);
        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            rawImg.preserveAspect = false;

        }
        else
        {
            //rawImg.transform.parent.transform.parent.gameObject.SetActive(true);

            Texture2D texture;
            texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            rawImg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            rawImg.preserveAspect = true;


        }
    }
}
