using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetPlayerNm : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNickNm()
    {

        if (GameManager.instance.nickNm == "")
        {
            GameObject playerSetPanel = GameObject.Find("MainCanvas").transform.Find("PlayerSetPanel").gameObject;
            GameManager.instance.nickNm = playerSetPanel.transform.Find("NickNmInputField").GetComponent<TMP_InputField>().text;
            GameManager.instance.originNickNm = playerSetPanel.transform.Find("NickNmInputField").GetComponent<TMP_InputField>().text;


            // 유저가 ID 입력 안했을 경우 체크하기위해 한번 더 체크
            if (GameManager.instance.nickNm == "")
            {
                return;
            }
            playerSetPanel.SetActive(false);
        }

        GameManager.instance.playerState = PlayerState.normal;
        GameManager.instance.playerPrefab.transform.Find("NickNm").transform.Find("NickNmText").GetComponent<TextMeshProUGUI>().text = GameManager.instance.originNickNm;


    }

    [PunRPC]
    public void SetNickNmPhoton(int viewID, string name)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PhotonView>().ViewID == viewID)
            {
                players[i].transform.Find("NickNm").transform.Find("NickNmText").GetComponent<TextMeshProUGUI>().text = name;
            }
        }
    }

    public void SetNickNmPhotonAll()
    {
        photonView.RPC("SetNickNmPhoton", RpcTarget.AllBuffered, GameManager.instance.viewID, GameManager.instance.nickNm);

    }
}
