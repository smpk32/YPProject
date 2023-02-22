using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using Unity.VideoHelper;

public class FrameSet : MonoBehaviour
{

    //string[] fileNm = { "sample1.jpg", "sample2.jpg" };

    int ImgCnt = 0;
    int nowCnt = 0;




    // Start is called before the first frame update
    void Start()
    {
        // string urlHead = "http://192.168.1.142:8060/resources/unity/StreamingAssets/";
        
        
        GameObject.Find("MainCanvas").gameObject.transform.Find("LoadingImage").gameObject.SetActive(true);
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

    IEnumerator LoadImageTexture(RawImage rawImg,string url,string fileNm, string fileInfo)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
            nowCnt++;
            
            if (nowCnt == ImgCnt)
            {
                GameObject.Find("LoadingImage").gameObject.SetActive(false);
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
                GameObject.Find("LoadingImage").gameObject.SetActive(false);
            }
        }
    }

}
