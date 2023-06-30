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

    // ��������󼼸���Ʈ
    [Serializable]
    class EventDtlListData
    {
        public string event_cntnts_sn;                  // ������ id
        public string event_id;                         // ��� id
        public string cntnts_nm;                        // ������ �̸�
        public string cntnts_dc;                        // ������ ����
        public string atfl_id;                          // ���� id

    }

    DataTable eventDtlListBaseTable;



    int ImgCnt = 0;
    int nowCnt = 0;


    // ���� ������ ��ȣ
    int pageNum = 10;
    
    // ���� ��ü ��
    int pageMaxCnt = 16;
    
    // ������ ������ ��ȣ 
    public int MaxPage = 2;

    // ���������� ��ư
    public GameObject nextBtn;
    
    // ���� ������ ��ư
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

        // �ӽ� ���� ���
        string title = "���� ��μ� ����";
        string info = "��μ�� ��ǳ�������� ��μ質������ ������ �����Դϴ�. ���� �̷ο� ���̶� �Ͽ� �񸮼���� �Ҹ��ϴ�.\n��μ� ������ �����Ҹ� �ҹ��ϰ� ������ ��� �� �ֽ��ϴ�. ���� ���� ���ϰų� Ư�������� ������, �������� ������ �����̱� ������ �ż��� ���� �ణ�� �絵�� �־ ��ôµ� �źΰ��� ���� ���� ���� �����ص� ��Ż�� ���� �ʾ� ��� ������ε� ��� �� �ֽ��ϴ�.";


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
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) //�ҷ����� ���� ��
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
            // �ӽ� ���� ���
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
        // �÷��̾� ���°� normal�϶����� ����
        if( GameManager.instance.playerState == PlayerState.normal)
        {
            floorBtnList.SetActive(chk);
        }
    }

}
