using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{

    RaycastHit hit;
    Ray ray;

    public TextMeshProUGUI dialogText;
    string sampleText;

    private IEnumerator coroutine;

#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void OpenUseURL(string idx);

#endif

    // Start is called before the first frame update
    void Start()
    {
      
        sampleText = "�ȳ��ϼ��� �̰��� ����û�Դϴ�. \n ���� ���� �ȳ��� ���� Ŭ���ϼ��� ";
      
        StartCoroutine(Typing(sampleText));

    }

    // Update is called once per frame
    void Update()
    {
        //ClickNPC();
    }


    //CameraRotateController.OnPointerUp() �� �ű�
    public void ClickNPC()
    {

        GameObject.Find("MainCanvas").transform.Find("NPCPanel").transform.gameObject.SetActive(true);
        GameManager.instance.playerState = PlayerState.setting;
    }

    public void CloseNPC()
    {
        GameObject.Find("MainCanvas").transform.Find("NPCPanel").transform.gameObject.SetActive(false);
        GameManager.instance.playerState = PlayerState.normal;
    }

    /*public void OpenURL(string url)
    {

        Application.OpenURL(url);
    }*/
   

    IEnumerator Typing(string text)
    {
        gameObject.transform.Find("TextBox").transform.gameObject.SetActive(true);
        foreach (char letter in text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        gameObject.transform.Find("TextBox").transform.gameObject.SetActive(false);
        dialogText.text = "";

        yield return new WaitForSeconds(3f);
        StartCoroutine(Typing(sampleText));
    }

    //npc  url �̺�Ʈ
    public void ClickOpenURL(string index) {

        Debug.Log("index: " + index);
#if UNITY_WEBGL && !UNITY_EDITOR
        OpenUseURL(index);
#endif

        string url = "";
        switch (index)
        {

            case "1":
                url = "https://www.google.co.kr/";
    
                break;
            case "2":
                url = "https://www.naver.com/";
    
                break;
            case "3":
                url = "https://www.daum.net/";
    
                break;
            case "4":
                url = "https://baro.lx.or.kr/lgstrsurv/lgstrsurvInfo02.do";
    
                break;
        }

        Application.OpenURL(url);
    }

}
