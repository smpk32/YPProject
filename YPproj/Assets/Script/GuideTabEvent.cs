using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class GuideTabEvent : MonoBehaviour
{
    //��� �� 
    public ToggleGroup guideTabToggle;
    //���������̼�
    public GameObject pagination;
    //��ũ�Ѻ� ���� ����ũ
    public GameObject mask;

    public GameObject turnButton;
    public void ClickGuideTab(int idx)
    {
        for (int i =0; i<5; i++)
        {
            pagination.transform.GetChild(i).transform.gameObject.SetActive(false);
            mask.transform.GetChild(i).transform.gameObject.SetActive(false);
            turnButton.transform.GetChild(i).transform.gameObject.SetActive(false);
            //���õ� �� ���ڻ� ����
            guideTabToggle.transform.GetChild(i).GetComponent<Toggle>().transform.Find("Text").transform.GetComponent<TextMeshProUGUI>().color = new Color32(255,255,255,255);
        }

        if (guideTabToggle.transform.GetChild(idx).GetComponent<Toggle>().isOn)
        {
            
            mask.transform.GetChild(idx).transform.gameObject.SetActive(true);
            pagination.transform.GetChild(idx).transform.gameObject.SetActive(true);
            guideTabToggle.transform.GetChild(idx).GetComponent<Toggle>().transform.Find("Text").transform.GetComponent<TextMeshProUGUI>().color = new Color32(67,82,145,255);
            turnButton.transform.GetChild(idx).transform.gameObject.SetActive(true);
        }


    }


}
