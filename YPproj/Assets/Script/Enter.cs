using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Gravitons.UI.Modal;
using System.Runtime.InteropServices;

public class Enter : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern bool BadWordCheck(string msg);

    [DllImport("__Internal")]
    private static extern void OpenFullScreen();
#endif
    string Check;
    string nmText;
    // Start is called before the first frame update
    void Start()
    { 
        //캐릭터 선택 체크
        if (GameManager.instance.firstCheck)
        {
            
            GameObject.Find("MainCanvas").transform.Find("PlayerSetPanel").transform.gameObject.SetActive(true);

            GameManager.instance.firstCheck = false;
        }
        else
        {
            GameManager.instance.CreateSingleChacracter();
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //캐릭터 생성 / 아이디 체크
    public void EnterRoom()
    {

        GameObject playerSetPanel = GameObject.Find("MainCanvas").transform.Find("PlayerSetPanel").gameObject;
        GameObject nf = playerSetPanel.transform.Find("Canvas").transform.Find("Form").transform.Find("NickNmInputField").gameObject;

        nmText = nf.GetComponent<TMP_InputField>().text;
        Check = Regex.Replace(nmText, @"[^a-zA-Z0-9가-힣]", "", RegexOptions.Singleline);
        Check = Regex.Replace(nmText, @"[^\w\.-]", "", RegexOptions.Singleline);

        GameObject modal = GameObject.Find("MainCanvas").transform.Find("ModalManager").transform.gameObject;
        if (modal.transform.childCount > 0)
        {
            return;
        }

        if (nmText.Equals(Check) != true)
        {

            
            ModalManager.Show("알림", "입력값은 최대 10자리 이며 특수문자는 사용하실 수 없습니다. \n 다시입력하세요.",
                       new[] { new ModalButton() { Text = "확인" } });

            modal.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);

            Debug.Log("특수문자 사용.");

            nf.GetComponent<TMP_InputField>().text = "";
            nmText = "";
            Check = "";
            return;
        }
#if UNITY_WEBGL && !UNITY_EDITOR
        else if (BadWordCheck(nmText))
        {
            ModalManager.Show("알림", "사용할 수 없는 이름입니다. \n 다시입력하세요.",
                       new[] { new ModalButton() { Text = "확인" } });
            
            modal.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);

            Debug.Log("나쁜 단어 사용.");

            nf.GetComponent<TMP_InputField>().text = "";
            nmText = "";
            Check = "";
            return;
        }
#endif
        else
        {
            GameManager.instance.CreateSingleChacracter();

#if UNITY_WEBGL && !UNITY_EDITOR
            OpenFullScreen();
#endif
            Debug.Log("캐릭터생성 성공");
        }
           
        




    }

}
