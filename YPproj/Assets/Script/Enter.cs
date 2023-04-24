using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterRoom()
    {


        GameManager.instance.CreateSingleChacracter();

        /*if (GameManager.instance.nickNm == "")
        {
            GameObject playerSetPanel = GameObject.Find("MainCanvas").transform.Find("PlayerSetPanel").gameObject;
            GameManager.instance.playerState = PlayerState.setting;
            playerSetPanel.gameObject.SetActive(true);


            //Panel.transform.Find("Canvas").transform.Find("Background").transform.Find("Form").transform.Find("PlayerSetBtn").GetComponent<Button>().onClick.AddListener(GameManager.instance.playerPrefab.GetComponent<SetPlayerNm>().SetNickNm);

        }
        else
        {

            GameManager.instance.playerPrefab.GetComponent<SetPlayerNm>().SetNickNm();
        }*/

        
    }

}
