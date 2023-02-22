using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrameInfo : MonoBehaviour
{
    public struct FrameDtlInfo
    {
        public string frameNm, frameInfo;

        public FrameDtlInfo(string _frameNm, string _frameInfo)
        {
            this.frameNm = _frameNm;
            this.frameInfo = _frameInfo;
        }
    }


    public GameObject frameDtlPanel;

    public FrameDtlInfo frameDtlInfo = new FrameDtlInfo(null,null);

    float time = 0f;

    

    void Start()
    {
        frameDtlPanel = GameObject.Find("MainCanvas").transform.Find("FrameDtlPanel").gameObject;
    }
    public void ShowFramePanel(bool isOn)
    {
        if (isOn && frameDtlInfo.frameNm != null)
        {
            frameDtlPanel.transform.Find("DtlRawImage").GetComponent<RawImage>().texture = gameObject.GetComponent<RawImage>().texture;
            frameDtlPanel.transform.Find("TitleText").GetComponent<TextMeshProUGUI>().text = frameDtlInfo.frameNm;
            frameDtlPanel.transform.Find("InfoText").GetComponent<TextMeshProUGUI>().text = frameDtlInfo.frameInfo;
            frameDtlPanel.SetActive(true);
            //StartCoroutine(FadeFramePanel(isOn));
            //GameObject.Find("Player").GetComponent<PlayerController>().playerState = PlayerState.normal;
        }
        else
        {
            frameDtlPanel.SetActive(false);
        }
        GameObject.Find("Player").GetComponent<PlayerController>().playerState = PlayerState.normal;
    }

    public void FrameClickEvent()
    {
        Action _action = ()=>ShowFramePanel(true);
        GameObject.Find("PlayerObj").GetComponent<PlayerNav>().MovingToTarget(gameObject, _action);
    }

    public IEnumerator FadeFramePanel(bool isOn)
    {
        time = 0;
        CanvasRenderer[] canvasList = frameDtlPanel.GetComponentsInChildren<CanvasRenderer>();
        while (time < 1f)
        {
            foreach(CanvasRenderer renderer in canvasList)
            {
                renderer.SetColor(new Color(1, 1, 1, time));
            }

            time += Time.deltaTime;
            yield return null;
        }

        
    }
}
