using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using Unity.VideoHelper;
using TMPro;

public class FrameSet : MonoBehaviour
{

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
    RawImage[] rawImgList;

    GameObject floorBtnList;

    Button[] floorBtnImgList;
    public enum ImgPath
    {
        Server,
        Local
    }

    public ImgPath imgPath = ImgPath.Server;


    private void Awake()
    {
        loadingPanel = GameObject.Find("MainCanvas").gameObject.transform.Find("LoadingImage").gameObject;

        rawImgList = gameObject.GetComponentsInChildren<RawImage>();
        
        topBarCanvas = gameObject.transform.Find("FloorCanvas").gameObject;

        //floorBtnImgList = topBarCanvas.GetComponentsInChildren<Button>();

        floorBtnList = topBarCanvas.transform.Find("FloorBtnList").gameObject;
        
        floorBtnImgList = floorBtnList.GetComponentsInChildren<Button>();

        floorTMP = topBarCanvas.transform.Find("TopBarRighTitle").transform.Find("Text").GetComponent<TextMeshProUGUI>();

    }

    // Start is called before the first frame update
    void Start()
    {
        FloorBtnInit();


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

    void FloorBtnInit()
    {
        for (int i = 0; i < floorBtnImgList.Length; i++)
        {
            floorBtnImgList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < MaxPage; i++)
        {
            floorBtnImgList[i].gameObject.SetActive(true);
            if(i == MaxPage - 1)
            {
                floorBtnImgList[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("SourceImg/bg-topBar-right");
            }
        }
    }

    public void SetFrameLocalImg()
    {
        loadingPanel.SetActive(true);
        topBarCanvas.SetActive(false);
        Invoke("ShowLoadingImg", 2.0f);
        nowCnt = 0;

        //PageCntTMP.GetComponent<TextMeshProUGUI>().text = (pageNum + 1).ToString();

        //string urlHead = "http://192.168.1.142:8080/files/";

        //RawImage[] list = gameObject.GetComponentsInChildren<RawImage>();
        //ImgCnt = list.Length;

        // 임시 정보 출력
        string title = "군민과 함께하는 소통한마당";
        string info = "'소통한마당'이란, 민선 8기 군정 계획을 공유하고 지역 현안에 대하여 주민과 소통·공감하는 자리입니다.";


        int imgIdx = pageNum * pageMaxCnt;
        for (int i = 0; i < pageMaxCnt; i++)
        {
            Texture tx = Resources.Load<Texture>("Image/sample" + (i + imgIdx));

            if(tx != null)
            {
                rawImgList[i].texture = tx;
                if (rawImgList[i].GetComponent<FrameInfo>() != null)
                {
                    rawImgList[i].transform.parent.transform.parent.gameObject.SetActive(true);
                    rawImgList[i].GetComponent<FrameInfo>().frameDtlInfo = new FrameInfo.FrameDtlInfo(title, info);
                }
            }
            else
            {
                rawImgList[i].transform.parent.transform.parent.gameObject.SetActive(false);
                rawImgList[i].texture = null;
                rawImgList[i].GetComponent<FrameInfo>().frameDtlInfo = new FrameInfo.FrameDtlInfo(null, null);
            }
            
            nowCnt++;

        }
    }




    // 페이징으로 액자 이미지 호출 함수
    public void SetFrameImg()
    {

        loadingPanel.SetActive(true);
        topBarCanvas.SetActive(false);
        Invoke("ShowLoadingImg", 2.0f);
        nowCnt = 0;

        //PageCntTMP.GetComponent<TextMeshProUGUI>().text = (pageNum + 1).ToString();

        string urlHead = "http://192.168.1.142:8080/files/";

        //RawImage[] list = gameObject.GetComponentsInChildren<RawImage>();

        // 임시 정보 출력
        string title = "군민과 함께하는 소통한마당";
        string info = "'소통한마당'이란, 민선 8기 군정 계획을 공유하고 지역 현안에 대하여 주민과 소통·공감하는 자리입니다.";


        int imgIdx = pageNum * pageMaxCnt;
        for (int i = 0; i < pageMaxCnt; i++)
        {
            StartCoroutine(LoadImageTexture(rawImgList[i], urlHead + "sample" + (i+ imgIdx) + ".jpg", title, info));
        }
    }



    IEnumerator LoadImageTexture(RawImage rawImg,string url,string fileNm, string fileInfo)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        nowCnt++;
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            
            rawImg.transform.parent.transform.parent.gameObject.SetActive(false);
            rawImg.texture = null;
            rawImg.GetComponent<FrameInfo>().frameDtlInfo = new FrameInfo.FrameDtlInfo(null, null);
            
        }
        else
        {
            rawImg.transform.parent.transform.parent.gameObject.SetActive(true);
            rawImg.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            if(rawImg.GetComponent<FrameInfo>() != null)
            {
                rawImg.GetComponent<FrameInfo>().frameDtlInfo = new FrameInfo.FrameDtlInfo(fileNm, fileInfo);
            }
            

        }


        /*if (nowCnt == pageMaxCnt)
        {
            loadingPanel.SetActive(false);
        }*/
    }

    /* 
     * nextChk가 true > 다음 페이지
     *          false > 이전 페이지
    */
    public void NextPage(bool nextChk)
    {
        pageNum = nextChk ? pageNum += 1 : pageNum -= 1;

        if(pageNum == 0)
        {
            prevBtn.SetActive(false);
        }
        else
        {
            prevBtn.SetActive(true);
        }

        if(pageNum == MaxPage-1)
        {
            nextBtn.SetActive(false);
        }
        else
        {
            nextBtn.SetActive(true);
        }
        if (imgPath == ImgPath.Server)
        {
            SetFrameImg();
        }
        else
        {
            SetFrameLocalImg();
        }
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

    public void FloorChange(int floor)
    {
        OpenFloorBtnList(false);

        if (pageNum == floor)
        {
            return;
        }

        pageNum = floor;

        floorTMP.text = (pageNum + 1).ToString() + "F";


        for (int i = 0; i< floorBtnImgList.Length; i++)
        {
            floorBtnImgList[i].gameObject.transform.Find("FloorImage").gameObject.SetActive(false);
        }

        floorBtnImgList[floor].gameObject.transform.Find("FloorImage").gameObject.SetActive(true);

        if (imgPath == ImgPath.Server)
        {
            SetFrameImg();
        }
        else
        {
            SetFrameLocalImg();
        }

    }

    public void OpenFloorBtnList(bool chk)
    {
        floorBtnList.SetActive(chk);
    }

}
