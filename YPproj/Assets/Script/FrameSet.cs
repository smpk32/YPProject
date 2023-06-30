using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using Unity.VideoHelper;
using TMPro;
using System;
using System.Data;

public class FrameSet : MonoBehaviour
{

    // 행사정보상세리스트
    [Serializable]
    class EventDtlListData
    {
        public string event_cntnts_sn;                  // 컨텐츠 id
        public string event_id;                         // 행사 id
        public string cntnts_nm;                        // 컨텐츠 이름
        public string cntnts_dc;                        // 컨텐츠 설명
        public string atfl_id;                          // 파일 id

    }

    DataTable eventDtlListBaseTable;



    int ImgCnt = 0;
    int nowCnt = 0;


    // 현재 페이지 번호
    int pageNum = 10;
    
    // 액자 전체 수
    int pageMaxCnt = 16;
    
    // 마지막 페이지 번호 
    public int MaxPage = 2;

    // 다음페이지 버튼
    public GameObject nextBtn;
    
    // 이전 페이지 버튼
    public GameObject prevBtn;

    GameObject loadingPanel;
    GameObject topBarCanvas;
    //public GameObject PageCntTMP;
    public TextMeshProUGUI floorTMP;
    Image[] rawImgList;

    GameObject floorBtnList;

    Button[] floorBtnImgList;

    public Sprite defaultTexture;

    public enum ImgPath
    {
        Server,
        Local
    }

    public ImgPath imgPath = ImgPath.Server;


    private void Awake()
    {
        loadingPanel = GameObject.Find("MainCanvas").gameObject.transform.Find("LoadingImage").gameObject;

        rawImgList = gameObject.GetComponentsInChildren<Image>();
        
        topBarCanvas = gameObject.transform.Find("FloorCanvas").gameObject;

        floorBtnList = topBarCanvas.transform.Find("FloorBtnList").gameObject;
        
        floorBtnImgList = floorBtnList.GetComponentsInChildren<Button>();

        floorTMP = topBarCanvas.transform.Find("TopBarRighTitle").transform.Find("Text").GetComponent<TextMeshProUGUI>();

    }

    // Start is called before the first frame update
    void Start()
    {
        //FloorBtnInit();


        //prevBtn.SetActive(false);
        /*if(imgPath == ImgPath.Server)
        {
            SetFrameImg();
        }
        else
        {
            SetFrameLocalImg();
        }*/
        //FloorChange(0);

    }

    public void SetFrameLocalImg()
    {
        loadingPanel.SetActive(true);
        topBarCanvas.SetActive(false);
        Invoke("ShowLoadingImg", 2.0f);
        nowCnt = 0;

        //PageCntTMP.GetComponent<TextMeshProUGUI>().text = (pageNum + 1).ToString();

        //string urlHead = "http://192.168.1.113:8080/files/";

        //RawImage[] list = gameObject.GetComponentsInChildren<RawImage>();
        //ImgCnt = list.Length;

        // 임시 정보 출력
        string title = "양평 고로쇠 축제";
        string info = "고로쇠란 단풍나무과인 고로쇠나무에서 나오는 수액입니다. 뼈에 이로운 물이라 하여 골리수라고 불립니다.\n고로쇠 수액은 남녀노소를 불문하고 누구나 드실 수 있습니다. 맛과 향이 진하거나 특별하지는 않지만, 나무에서 나오는 수액이기 때문에 신선한 향기와 약간의 당도가 있어서 드시는데 거부감이 없고 많은 양을 섭취해도 배탈이 나질 않아 기수 대용으로도 드실 수 있습니다.";


        int imgIdx = pageNum * pageMaxCnt;
        for (int i = 0; i < pageMaxCnt; i++)
        {
            Texture2D tx = Resources.Load<Texture2D>("Image/sample" + (i + imgIdx));

            if(tx != null)
            {
                
                rawImgList[i].sprite = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), Vector2.zero);
                rawImgList[i].preserveAspect = true;
                if (rawImgList[i].GetComponent<FrameInfo>() != null)
                {
                    rawImgList[i].transform.parent.transform.parent.gameObject.SetActive(true);
                    rawImgList[i].GetComponent<FrameInfo>().frameDtlInfo = new FrameInfo.FrameDtlInfo(title, info);
                }
            }
            else
            {
                //rawImgList[i].transform.parent.transform.parent.gameObject.SetActive(false);
                //rawImgList[i].texture = null;
                rawImgList[i].GetComponent<FrameInfo>().frameDtlInfo = new FrameInfo.FrameDtlInfo(null, null);
            }
            
            nowCnt++;

        }
    }

    public void EnterGallery()
    {
        if (imgPath == ImgPath.Server)
        {
            //SetFrameImg();
            StartCoroutine(SelectEventDtlList());

            SetMainPoster(GameManager.instance.eventFileId,GameManager.instance.eventNm,GameManager.instance.eventDc);



        }
        else
        {
            SetFrameLocalImg();
        }
    }

    public IEnumerator SelectEventDtlList()
    {
        string GetDataUrl = GameManager.Instance.baseURL + "/event/select?eventId="+GameManager.instance.eventId;

        using (UnityWebRequest www = UnityWebRequest.Get(GetDataUrl))
        {
            yield return www.SendWebRequest();
            // yield return System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) //불러오기 실패 시
            {
                Debug.Log(www.error);
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
                        //callback(System.Text.Encoding.UTF8.GetString(www.downloadHandler.data));
                        //yield return System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);

                        var dataSet = JsonConvert.DeserializeObject<List<EventDtlListData>>(System.Text.Encoding.UTF8.GetString(www.downloadHandler.data));

                        DataTable eventListTable = new DataTable();
                        eventListTable.Columns.Add(new DataColumn("event_cntnts_sn", typeof(string)));
                        eventListTable.Columns.Add(new DataColumn("event_id", typeof(string)));
                        eventListTable.Columns.Add(new DataColumn("cntnts_nm", typeof(string)));
                        eventListTable.Columns.Add(new DataColumn("cntnts_dc", typeof(string)));
                        eventListTable.Columns.Add(new DataColumn("atfl_id", typeof(string)));

                        //MaxPage = ((int)Math.Ceiling((float)(dataSet.Count / pageMaxCnt)) == 0) ? 1 : (int)Math.Ceiling((float)(dataSet.Count / pageMaxCnt));

                        MaxPage = (int)Mathf.FloorToInt(dataSet.Count / pageMaxCnt);


                        for (int i = 0; i < dataSet.Count; i++)
                        {
                            DataRow row = eventListTable.NewRow();

                            row["event_cntnts_sn"] = dataSet[i].event_cntnts_sn;
                            row["event_id"] = dataSet[i].event_id;
                            row["cntnts_nm"] = dataSet[i].cntnts_nm;
                            row["cntnts_dc"] = dataSet[i].cntnts_dc;
                            row["atfl_id"] = dataSet[i].atfl_id;

                            eventListTable.Rows.Add(row);
                        }

                        eventDtlListBaseTable = eventListTable;

                        FloorBtnInit();

                        

                    }

                }
            }
        }
    }

    void FloorBtnInit()
    {
        pageNum = 10;
        for (int i = 0; i < floorBtnImgList.Length; i++)
        {
            floorBtnImgList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < MaxPage+1; i++)
        {
            floorBtnImgList[i].gameObject.SetActive(true);
            if(i == MaxPage)
            {
                floorBtnImgList[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("SourceImg/bg-topBar-right");
            }
        }
        FloorChange(0);
    }

    public void FloorChange(int floor)
    {
        OpenFloorBtnList(false);

        System.GC.Collect();
        Resources.UnloadUnusedAssets();

        if (pageNum == floor)
        {
            return;
        }
        //loadingPanel.SetActive(true);

        pageNum = floor;

        

        floorTMP.text = (pageNum + 1).ToString() + "F";


        for (int i = 0; i < floorBtnImgList.Length; i++)
        {
            floorBtnImgList[i].gameObject.transform.Find("FloorImage").gameObject.SetActive(false);
        }

        floorBtnImgList[floor].gameObject.transform.Find("FloorImage").gameObject.SetActive(true);

        if (imgPath == ImgPath.Server)
        {
            StartCoroutine(SetServerImg());
            //StartCoroutine(SelectEventDtlList());
        }
        else
        {
            SetFrameLocalImg();
        }

    }

    public IEnumerator SetServerImg()
    {
        int startCnt = (pageNum == 0) ? 0 : (pageMaxCnt * pageNum );

        nowCnt = 0;
        ImgCnt = 0;

        for (int i = startCnt; i < startCnt+(pageMaxCnt); i++)
        {
            rawImgList[nowCnt].preserveAspect = false;
            rawImgList[nowCnt].sprite = defaultTexture;
            if (i >= eventDtlListBaseTable.Rows.Count)
            {
                rawImgList[nowCnt].GetComponent<FrameInfo>().frameDtlInfo = new FrameInfo.FrameDtlInfo(null, null);
                nowCnt++;
                ImgCnt++;
                continue;
            }


            DataRow row = eventDtlListBaseTable.Rows[i];
            // 임시 정보 출력
            string eventId = row["event_id"].ToString();
            string atflId = row["atfl_id"].ToString();
            string cntntsNm = row["cntnts_nm"].ToString();
            string cntntsDc = row["cntnts_dc"].ToString();
            yield return new WaitForSecondsRealtime(0.1f);
            StartCoroutine(LoadImageTexture(rawImgList[nowCnt], atflId, cntntsNm, cntntsDc));

            nowCnt++;

        }
        
        //loadingPanel.SetActive(false);
    }


    public IEnumerator LoadImageTexture(Image rawImg,string fileNm, string cntntsNm, string cntntsDc)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(GameManager.instance.baseURL+ "/display?filename=" + fileNm,true);
        yield return www.SendWebRequest();
        
        if (!rawImg.name.Equals("PosterImage"))
        {
            ImgCnt++;
        }


        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);

            //rawImg.transform.parent.transform.parent.gameObject.SetActive(false);
            //rawImg.texture = null;
            rawImg.sprite = defaultTexture;
            //rawImg.GetComponent<FrameInfo>().frameDtlInfo = new FrameInfo.FrameDtlInfo(null, null);

            if (cntntsNm != null)
            {
                rawImg.GetComponent<FrameInfo>().frameDtlInfo = new FrameInfo.FrameDtlInfo(cntntsNm, cntntsDc);
            }
            rawImg.preserveAspect = false;

        }
        else
        {
            rawImg.transform.parent.transform.parent.gameObject.SetActive(true);

            Texture2D texture;
            texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            
            if(texture != null) { 
                rawImg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                rawImg.preserveAspect = true;
            }

            if (rawImg.GetComponent<FrameInfo>() != null)
            {
                rawImg.GetComponent<FrameInfo>().frameDtlInfo = new FrameInfo.FrameDtlInfo(cntntsNm, cntntsDc);
            }
            

        }

        if(ImgCnt == pageMaxCnt)
        {
            loadingPanel.SetActive(false);
        }



    }

    public void SetMainPoster(string fileId, string eventNm, string eventDc)
    {
        Image posterImg = GameObject.Find("Map").transform.Find("GalleryGrp (1)").transform.Find("Wall").transform.Find("Poster").transform.Find("PosterImage").GetComponent<Image>();
        posterImg.sprite = defaultTexture;
        posterImg.preserveAspect = true;

        GameObject.Find("Map").transform.Find("GalleryGrp (1)").transform.Find("FrameGrp").transform.Find("FloorCanvas").transform.Find("TopBarMiddle").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = GameManager.instance.eventNm;

        StartCoroutine(LoadImageTexture(posterImg, fileId, eventNm, eventDc));

    }

    



    public void ShowLoadingImg()
    {
        if (nowCnt == pageMaxCnt)
        {
            loadingPanel.SetActive(false);
            topBarCanvas.SetActive(true);
        }
        else
        {
            Invoke("ShowLoadingImg", 2.0f);
        }
    }

    

    public void OpenFloorBtnList(bool chk)
    {
        // 플레이어 상태가 normal일때에만 실행
        if( GameManager.instance.playerState == PlayerState.normal)
        {
            floorBtnList.SetActive(chk);
        }
    }

}
