using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VideoHelper;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoCtrl : MonoBehaviour
{
    public Texture videoRenderTexture;
    public VideoPlayer vp;
    public RawImage videoRawImg;

    public string videoUrl = "";



    void Awake()
    {
        //GetVideoRoot();
    }


    public void GetVideoRoot()
    {
        //바꿔야됨
        videoUrl = GameManager.instance.baseURL+ "/display?filename=";
        LoadVideo();

    }


    public void SetVideoUrl(string url)
    {
        Debug.Log(url);
        videoUrl = url;
        LoadVideo();
    }

    void OnEnable()
    {
        GetVideoRoot();
    }


    /*public void LoadVideo(string url)
    {
        videoRawImg.texture = videoRenderTexture;
        vp.url = url;
        vp.prepareCompleted += Prepared;
        vp.Prepare();


        var fitter = videoRawImg.gameObject.GetOrAddComponent<AspectRatioFitter>();
        //fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;

        fitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;

        fitter.aspectRatio = (float)videoRenderTexture.width / videoRenderTexture.height;
        

    }*/

    public void LoadVideo()
    {
        videoRawImg.texture = videoRenderTexture;
        //버튼 클릭했을때 파일 네임 받아오기
        vp.url = videoUrl + GameManager.instance.inhbtntPranAtflId;

        Debug.Log(videoUrl + GameManager.instance.inhbtntPranAtflId);
     
        vp.prepareCompleted += Prepared;
        vp.Prepare();

        var fitter = videoRawImg.gameObject.GetOrAddComponent<AspectRatioFitter>();
        

        fitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;

        fitter.aspectRatio = (float)videoRenderTexture.width / videoRenderTexture.height;


    }

    void Prepared(VideoPlayer vp)
    {
        //vp.Pause();
        vp.Play();
    }

    public void VideoControl()
    {

        if (vp.isPlaying)
        {
            vp.Pause();
        }
        else if (vp.isPaused)
        {
            vp.Play();
        }

    }
}
