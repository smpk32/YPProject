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
    Image[] rawImgList;

    GameObject floorBtnList;

    Button[] floorBtnImgList;

    public Texture defaultTexture;

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




    // 페이징으로 액자 이미지 호출 함수
    public void SetFrameImg()
    {

        loadingPanel.SetActive(true);
        topBarCanvas.SetActive(false);
        Invoke("ShowLoadingImg", 2.0f);
        nowCnt = 0;

        //PageCntTMP.GetComponent<TextMeshProUGUI>().text = (pageNum + 1).ToString();

        string urlHead = "http://192.168.1.113:8080/selectImg?file_nm=";

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



    IEnumerator LoadImageTexture(Image rawImg,string url,string fileNm, string fileInfo)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        nowCnt++;
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            
            //rawImg.transform.parent.transform.parent.gameObject.SetActive(false);
            //rawImg.texture = null;
            rawImg.GetComponent<FrameInfo>().frameDtlInfo = new FrameInfo.FrameDtlInfo(null, null);
            rawImg.preserveAspect = false;

        }
        else
        {
            rawImg.transform.parent.transform.parent.gameObject.SetActive(true);

            Texture2D texture;
            texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            rawImg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            rawImg.preserveAspect = true;
            if (rawImg.GetComponent<FrameInfo>() != null)
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
        // 플레이어 상태가 normal일때에만 실행
        if( GameManager.instance.playerState == PlayerState.normal)
        {
            floorBtnList.SetActive(chk);
        }
    }

}
