using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatImgEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowChatImg()
    {
        Sprite sp = gameObject.transform.Find("Img").GetComponent<Image>().sprite;
        
        // sprite 이미지를 texture로 변환
        Texture2D newtexture = new Texture2D((int)sp.rect.width,(int)sp.rect.height);
        Color[] newColors = sp.texture.GetPixels((int)sp.textureRect.x,(int)sp.textureRect.y,(int)sp.textureRect.width,(int)sp.textureRect.height);
        newtexture.SetPixels(newColors);
        newtexture.Apply();

        GameObject frameDtlPanel = GameObject.Find("MainCanvas").transform.Find("FrameDtlPanel").gameObject;

        frameDtlPanel.transform.Find("DtlRawImage").GetComponent<RawImage>().texture = newtexture;


        frameDtlPanel.SetActive(true);
    }
}
