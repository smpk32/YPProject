using System.Collections;
using System.Collections.Generic;
using Unity.VideoHelper;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoCtrl : MonoBehaviour
{
    public Texture videoRenderTexture;
    public VideoPlayer vp;
    public RawImage videoRawImg;
    // Start is called before the first frame update
    void Start()
    {
        //string urlHead = "http://192.168.1.142:8060/resources/unity/StreamingAssets/";
        //string urlHead = "http://192.168.1.142:8080/files/";
        //LoadVideo(urlHead + "yangpyeongAD.mp4");
    }


    public void LoadVideo(string url)
    {
        videoRawImg.texture = videoRenderTexture;
        vp.url = url;
        vp.prepareCompleted += Prepared;
        vp.Prepare();


        var fitter = videoRawImg.gameObject.GetOrAddComponent<AspectRatioFitter>();
        //fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;

        fitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;

        fitter.aspectRatio = (float)videoRenderTexture.width / videoRenderTexture.height;
        

    }

    public void LoadVideo2()
    {
        videoRawImg.texture = videoRenderTexture;
#if UNITY_WEBGL && !UNITY_EDITOR
        //vp.url = "http://192.168.1.142:8080/metaverse/StreamingAssets/YP2023.mp4";
        vp.url = "http://203.228.54.47/metabus/StreamingAssets/YP2023.mp4";
#else
        //vp.url = vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "YP2023.mp4");
        vp.url = "http://203.228.54.47/metabus/StreamingAssets/YP2023.mp4";
#endif
        //vp.url = vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "yangpyeongAD.mp4");
        vp.prepareCompleted += Prepared;
        vp.Prepare();


        var fitter = videoRawImg.gameObject.GetOrAddComponent<AspectRatioFitter>();
        //fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;

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
