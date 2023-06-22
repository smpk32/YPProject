using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UI.Extensions;

public class FormController : MonoBehaviour
{

    public TMP_Dropdown dropdown;
    public ToggleGroup toggleGroup;
    GameObject leftpanelImage;

    // Start is called before the first frame update
    void Start()
    {
        leftpanelImage = GameObject.Find("MainCanvas").transform.Find("PlayerSetPanel").transform.Find("Background").transform.Find("Box_left").transform.Find("CharacterBox").transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CreateConImg(int i, Component path, Toggle toggle) 
    {
        
        GameObject img = Instantiate(Resources.Load<GameObject>("Prefabs\\CharacterImg"));
        //img.transform.SetParent(path.transform);

        img.name = "Img" + i;
        gameObject.transform.Find("Mask").transform.Find("HorizontalScrollSnap").GetComponent<HorizontalScrollSnap>().AddChild(img);

          
        //img.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterImg\\" + toggle.name + (i + 1));
        img.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterImg\\" + toggle.name + (i + 1)+ "_top");

    }
    //남자, 여자 선택
    public void ChangeVar()
    {
        //삭제
       // int selVal = dropdown.value;

         
       //선택된 토글값
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();

        Component path = gameObject.transform.Find("Mask").transform.Find("HorizontalScrollSnap").transform.Find("Content");
        HorizontalScrollSnap scrollSnap = gameObject.transform.Find("Mask").transform.Find("HorizontalScrollSnap").GetComponent<HorizontalScrollSnap>();

        Image[] childList = path.GetComponentsInChildren<Image>();
        //
        if (childList != null)
        {
            for (int i = 0; i < childList.Length; i++)
            {
                
                Destroy(childList[i].gameObject);
            }

        }

        scrollSnap.RemoveAllChildren(out scrollSnap.ChildObjects);
        //상단이미지 변경
        leftpanelImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterImg\\" + toggle.name + "1");
        //하단이미지 변경

        if (toggle.name == "ManPlayer")
        {
            
            GameManager.Instance.selectCharacter = "ManPlayer1";

            for (int i = 0; i < 3; i++)
            {
                CreateConImg(i, path, toggle);
            }
        
        }
        else
        {
             
            GameManager.Instance.selectCharacter = "WomenPlayer1";
            for (int i = 0; i < 3; i++)
            {
                CreateConImg(i, path, toggle);
            }
            
        }

        GameObject con = GameObject.Find("MainCanvas").transform.Find("PlayerSetPanel").transform.Find("Canvas").transform.Find("Mask").transform.Find("HorizontalScrollSnap").transform.Find("Content").transform.gameObject; 
        con.transform.GetChild(0).Find("Image").GetComponent<Outline>().enabled = true;
    }

}
