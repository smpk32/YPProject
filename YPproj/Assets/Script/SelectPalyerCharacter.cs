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

    //ĳ���� ������ ���� �̺�Ʈ  
    public void ClickFreeView()
    {
        //������ ���ý� �߾� �����ӿ� �ø���
        Image selectedImage = this.gameObject.transform.Find("Image").GetComponent<Image>();
        imageBox.sprite = selectedImage.sprite;

        //ĳ���� �̸� ����
        GameManager.Instance.selectCharacter = selectedImage.sprite.name;


    }
}
