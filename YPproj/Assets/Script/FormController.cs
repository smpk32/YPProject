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
    

    // Start is called before the first frame update
    void Start()
    {

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
        Debug.Log("CharacterImg\\" + toggle.name + (i + 1));
        img.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterImg\\" + toggle.name + (i + 1));

    }

    public void ChangeVar()
    {

        int selVal = dropdown.value;

        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();

        Component path = gameObject.transform.Find("Mask").transform.Find("HorizontalScrollSnap").transform.Find("Content");
        HorizontalScrollSnap scrollSnap = gameObject.transform.Find("Mask").transform.Find("HorizontalScrollSnap").GetComponent<HorizontalScrollSnap>();

        Image[] childList = path.GetComponentsInChildren<Image>();
        Debug.Log(childList.Length);
        if (childList != null)
        {
            for (int i = 0; i < childList.Length; i++)
            {
                Debug.Log(childList[i].gameObject);
                Destroy(childList[i].gameObject);
            }

        }

        scrollSnap.RemoveAllChildren(out scrollSnap.ChildObjects);

        if (toggle.name == "ManPlayer")
        {

            if (selVal == 0)
            {

                for (int i=0; i<2; i++)
                {
                    CreateConImg(i, path, toggle);
                }
            }
            else if(selVal == 1)
            {
               
                for (int i = 2; i < 4; i++)
                {
                    CreateConImg(i, path, toggle);
                }
            }
            else if (selVal == 2)
            {
                for (int i = 4; i < 6; i++)
                {
                    CreateConImg(i, path, toggle);
                }
            }
            else if (selVal == 3)
            {
                for (int i = 6; i < 8; i++)
                {
                    CreateConImg(i, path, toggle);
                }
            }
        }
        else
        {
            if (selVal == 0)
            {

                for (int i = 0; i < 2; i++)
                {
                    CreateConImg(i, path, toggle);
                }
            }
            else if (selVal == 1)
            {

                for (int i = 2; i < 4; i++)
                {
                    CreateConImg(i, path, toggle);
                }
            }
            else if (selVal == 2)
            {
                for (int i = 4; i < 6; i++)
                {
                    CreateConImg(i, path, toggle);
                }
            }
            else if (selVal == 3)
            {
                for (int i = 6; i < 8; i++)
                {
                    CreateConImg(i, path, toggle);
                }
            }
        }
    }

}
