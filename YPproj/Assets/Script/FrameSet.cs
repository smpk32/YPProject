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

        // �ӽ÷� �ε�ȭ�� true
        loadingPanel.SetActive(true);

        string urlHead = "http://192.168.1.142:8080/files/";

        RawImage[] list = gameObject.GetComponentsInChildren<RawImage>();

        ImgCnt = list.Length;

        // �ӽ� ���� ���
        string title = "���ΰ� �Բ��ϴ� �����Ѹ���";
        string info = "'�����Ѹ���'�̶�, �μ� 8�� ���� ��ȹ�� �����ϰ� ���� ���ȿ� ���Ͽ� �ֹΰ� ���롤�����ϴ� �ڸ��Դϴ�.";


        for (int i = 0; i<list.Length; i++)
        {
            StartCoroutine(LoadImageTexture(list[i], urlHead + "sample"+i+".jpg", title, info));
        }

    }

    // ����¡���� ���� �̹��� ȣ�� ����
    public void SetFrameImg()
    {

        loadingPanel.SetActive(true);
        nowCnt = 0;

        PageCntTMP.GetComponent<TextMeshProUGUI>().text = (pageNum + 1).ToString();

        string urlHead = "http://192.168.1.142:8080/files/";

        RawImage[] list = gameObject.GetComponentsInChildren<RawImage>();

        // �ӽ� ���� ���
        string title = "���ΰ� �Բ��ϴ� �����Ѹ���";
        string info = "'�����Ѹ���'�̶�, �μ� 8�� ���� ��ȹ�� �����ϰ� ���� ���ȿ� ���Ͽ� �ֹΰ� ���롤�����ϴ� �ڸ��Դϴ�.";


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
