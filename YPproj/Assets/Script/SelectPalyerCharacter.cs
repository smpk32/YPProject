using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPalyerCharacter : MonoBehaviour
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
        //프리뷰 선택시 중앙 프레임에 올리기
        Image selectedImage = this.gameObject.transform.Find("Image").GetComponent<Image>();
        imageBox.sprite = selectedImage.sprite;

        //캐릭터 이름 저장
        GameManager.Instance.selectCharacter = selectedImage.sprite.name;


    }
}
