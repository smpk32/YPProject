using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerCharacter : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject characterView;

    Image imageBox;

    void Start()
    {
        imageBox = GameObject.Find("MainCanvas").transform.Find("PlayerSetPanel").transform.Find("Background").transform.Find("Box_left").transform.Find("CharacterBox").GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ĳ���� ������ ���� �̺�Ʈ  
    public void ClickFreeView()
    {
        GameObject con = GameObject.Find("MainCanvas").transform.Find("PlayerSetPanel").transform.Find("Canvas").transform.Find("Mask").transform.Find("HorizontalScrollSnap").transform.Find("Content").transform.gameObject;
        int size = con.transform.childCount;
        
        for(int i =0; i< size; i++)
        {
            con.transform.GetChild(i).transform.Find("Image").GetComponent<Outline>().enabled = false ;
        }

        //������ ���ý� �߾� �����ӿ� �ø���
        Image selectedImage = this.gameObject.transform.Find("Image").GetComponent<Image>();
        
        //imageBox.sprite = selectedImage.sprite;
        imageBox.sprite = Resources.Load<Sprite>("CharacterImg\\" + selectedImage.sprite.name.Replace("_top",""));
        //���ý� �ƿ����� �׷��ֱ�
        this.gameObject.transform.Find("Image").GetComponent<Outline>().enabled = true;

        //ĳ���� �̸� ����
        GameManager.Instance.selectCharacter = selectedImage.sprite.name.Replace("_top", "");


    }
}
