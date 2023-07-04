using Photon.Pun;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using Gravitons.UI.Modal;

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
        GameObject playerSetPanel = GameObject.Find("MainCanvas").transform.Find("PlayerSetPanel").gameObject;

        GameObject nf = playerSetPanel.transform.Find("Canvas").transform.Find("Form").transform.Find("NickNmInputField").gameObject;


        // 처음 입장 후 입력값없을 때
        if (nf.GetComponent<TMP_InputField>().text == "" && GameManager.instance.nickNm.Equals(""))
        {
            GameManager.instance.nickNm = "사용자";
            GameManager.instance.originNickNm = "사용자";
        }
        // 입력값있을 때
        else if(nf.GetComponent<TMP_InputField>().text != "")
        {
            GameManager.instance.nickNm = nf.GetComponent<TMP_InputField>().text;
            GameManager.instance.originNickNm = nf.GetComponent<TMP_InputField>().text;
        }

        GameManager.instance.playerState = PlayerState.normal;
        

        GameManager.instance.playerPrefab.transform.Find("NickNm").transform.Find("NickNmText").GetComponent<TextMeshProUGUI>().text = GameManager.instance.originNickNm;

        playerSetPanel.SetActive(false);

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
