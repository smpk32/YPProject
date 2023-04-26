using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Gravitons.UI.Modal;
public class Enter : MonoBehaviour
{
    string Check;
    string nmText;
    // Start is called before the first frame update
    void Start()
    { 
        //ĳ���� ���� üũ
        if (GameManager.instance.firstCheck)
        {
            
            GameObject.Find("MainCanvas").transform.Find("PlayerSetPanel").transform.gameObject.SetActive(true);

            GameManager.instance.firstCheck = false;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterRoom()
    {

        GameObject playerSetPanel = GameObject.Find("MainCanvas").transform.Find("PlayerSetPanel").gameObject;
        GameObject nf = playerSetPanel.transform.Find("Canvas").transform.Find("Form").transform.Find("NickNmInputField").gameObject;

        nmText = nf.GetComponent<TMP_InputField>().text;
        Check = Regex.Replace(nmText, @"[^a-zA-Z0-9��-�R]", "", RegexOptions.Singleline);
        Check = Regex.Replace(nmText, @"[^\w\.-]", "", RegexOptions.Singleline);
        
        if (nmText.Equals(Check) != true)
        {
           ModalManager.Show("�˸�", "�Է°��� �ִ� 10�ڸ� �̸� Ư�����ڴ� ����Ͻ� �� �����ϴ�. \n �ٽ��Է��ϼ���.",
                       new[] { new ModalButton() { Text = "Ȯ��" } });
            GameObject modal = GameObject.Find("MainCanvas").transform.Find("ModalManager").transform.gameObject;
            modal.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);

            Debug.Log("Ư������ ���.");

            nf.GetComponent<TMP_InputField>().text = "";
            nmText = "";
            Check = "";
            return;
        }
        else
        {
        GameManager.instance.CreateSingleChacracter();
            Debug.Log("ĳ���ͻ��� ����");
        }
           
        




    }

}
