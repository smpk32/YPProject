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

    //캐릭터 프리뷰 선택 이벤트  
    public void ClickFreeView()
    {
        GameObject con = GameObject.Find("MainCanvas").transform.Find("PlayerSetPanel").transform.Find("Canvas").transform.Find("Mask").transform.Find("HorizontalScrollSnap").transform.Find("Content").transform.gameObject;
        int size = con.transform.childCount;
        
        for(int i =0; i< size; i++)
        {
            con.transform.GetChild(i).transform.Find("Image").GetComponent<Outline>().enabled = false ;
        }

        //프리뷰 선택시 중앙 프레임에 올리기
        Image selectedImage = this.gameObject.transform.Find("Image").GetComponent<Image>();
        
        //imageBox.sprite = selectedImage.sprite;
        imageBox.sprite = Resources.Load<Sprite>("CharacterImg\\" + selectedImage.sprite.name.Replace("_top",""));
        //선택시 아웃라인 그려주기
        this.gameObject.transform.Find("Image").GetComponent<Outline>().enabled = true;

        //캐릭터 이름 저장
        GameManager.Instance.selectCharacter = selectedImage.sprite.name.Replace("_top", "");


    }
}
