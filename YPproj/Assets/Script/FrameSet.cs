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

    //string[] fileNm = { "sample1.jpg", "sample2.jpg" };

    int ImgCnt = 0;
    int nowCnt = 0;

    int pageNum = 0;
    int pageMaxCnt = 8;
    int MaxPage = 3;

    public GameObject nextBtn;
    public GameObject prevBtn;

    GameObject loadingPanel;
    public GameObject PageCntTMP;




    // Start is called before the first frame update
    void Start()
    {
        // string urlHead = "http://192.168.1.142:8060/resources/unity/StreamingAssets/";


        loadingPanel = GameObject.Find("MainCanvas").gameObject.transform.Find("LoadingImage").gameObject;


        //prevBtn.SetActive(false);
        //SetFrameImg();

        // 임시로 로딩화면 true
        loadingPanel.SetActive(true);

        string urlHead = "http://192.168.1.142:8080/files/";

        RawImage[] list = gameObject.GetComponentsInChildren<RawImage>();

        ImgCnt = list.Length;

        // 임시 정보 출력
        string title = "군민과 함께하는 소통한마당";
        string info = "'소통한마당'이란, 민선 8기 군정 계획을 공유하고 지역 현안에 대하여 주민과 소통·공감하는 자리입니다.";


        for (int i = 0; i<list.Length; i++)
        {
            StartCoroutine(LoadImageTexture(list[i], urlHead + "sample"+i+".jpg", title, info));
        }

    }

    // 페이징으로 액자 이미지 호출 샘플
    public void SetFrameImg()
    {

        loadingPanel.SetActive(true);
        nowCnt = 0;

        PageCntTMP.GetComponent<TextMeshProUGUI>().text = (pageNum + 1).ToString();

        string urlHead = "http://192.168.1.142:8080/files/";

        RawImage[] list = gameObject.GetComponentsInChildren<RawImage>();

        // 임시 정보 출력
        string title = "군민과 함께하는 소통한마당";
        string info = "'소통한마당'이란, 민선 8기 군정 계획을 공유하고 지역 현안에 대하여 주민과 소통·공감하는 자리입니다.";


        int imgIdx = pageNum * pageMaxCnt;
        for (int i = 0; i < 8; i++)
        {
            StartCoroutine(LoadImageTexture(list[i], urlHead + "sample" + (i+ imgIdx) + ".jpg", title, info));
            Debug.Log(i + imgIdx);
        }
    }

    IEnumerator LoadImageTexture(RawImage rawImg,string url,string fileNm, string fileInfo)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            nowCnt++;
            rawImg.texture = null;
            if (nowCnt == ImgCnt)
            {
                loadingPanel.SetActive(false);
            }
        }
        else
        {
            rawImg.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            if(rawImg.GetComponent<FrameInfo>() != null)
            {
                rawImg.GetComponent<FrameInfo>().frameDtlInfo = new FrameInfo.FrameDtlInfo(fileNm, fileInfo);
            }
            nowCnt++;
            
            if (nowCnt == ImgCnt)
            {
                loadingPanel.SetActive(false);
            }
        }
    }

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
        Debug.Log("pageNum : "+pageNum);
        SetFrameImg();
    }

}
