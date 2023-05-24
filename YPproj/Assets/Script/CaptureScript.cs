using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CaptureScript : MonoBehaviour
{
    public Camera fullCam;       //보여지는 카메라.
    public Camera topCam;       //보여지는 카메라.

    private int resWidth;
    private int resHeight;
    string path;
    // Use this for initialization
    void Start()
    {
        fullCam = GameObject.Find("FullCamera").GetComponent<Camera>();
        topCam = GameObject.Find("TopCamera").GetComponent<Camera>();

        resWidth = Screen.width;
        resHeight = Screen.height;
        path = Application.dataPath + "/ScreenShot/";
        Debug.Log(path);
    }

    public void ClickScreenShot(Camera cam)
    {
        Camera captureCam = cam;
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }
        string name;
        name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        captureCam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        captureCam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(name, bytes);

        //ClickScreenShot(topCam);
    }

    public void StartCapture()
    {
        ClickScreenShot(fullCam);
        //ClickScreenShot(topCam);
    }

    public void StartCapture2()
    {
        //ClickScreenShot(fullCam);
        ClickScreenShot(topCam);
    }

}
