using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviourPunCallbacks
{

    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    //string roomNum = "";

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance.nickNm == "")
        {
            GameObject playerSetPanel = GameObject.Find("MainCanvas").transform.Find("PlayerSetPanel").gameObject;
            GameManager.instance.playerState = PlayerState.setting;
            playerSetPanel.gameObject.SetActive(true);

            playerSetPanel.transform.Find("PlayerSetBtn").GetComponent<Button>().onClick.AddListener(GameManager.instance.playerPrefab.GetComponent<SetPlayerNm>().SetNickNm);
        }
        else
        {
            GameManager.instance.playerPrefab.GetComponent<SetPlayerNm>().SetNickNm();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enter()
    {
        /*PhotonNetwork.GameVersion = "1";
        AuthenticationValues authValues = new AuthenticationValues("");
        PhotonNetwork.AuthValues = authValues;
        PhotonNetwork.NickName = inputName.text;*/

        //������ ������ ������ ���� ���� �õ�
        if (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
        {
            PhotonNetwork.Disconnect();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        //�� ���� ��ư Ȱ��ȭ

        //EnterButton.interactable = true;
        //���� ���� ǥ��
        //connectionInfoText.text = "�¶���: ������ ������ �����";
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        //PhotonNetwork.FindFriends(new string[] { inputName.text });
        //StartCoroutine(DBtest(inputName.text, InputCompany.text));

        Connect();
    }

    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        base.OnFriendListUpdate(friendList);
        Debug.Log("OnFriendListUpdate");
        foreach (FriendInfo info in friendList)
        {
            if (info.IsOnline && info.IsInRoom)
            {
                /*GameObject LobbyCanvas = GameObject.Find("CanvasGroup").transform.Find("LobbyCanvas").gameObject;
                LobbyCanvas.SetActive(false);*/
                /*EnterButton.interactable = true;
                //���� �õ� ������ �ؽ�Ʈ�� ǥ��
                connectionInfoText.text = "�̹� �α��ε� �����Դϴ�.";*/
                Debug.Log("OnFriendListUpdate2");
                PhotonNetwork.Disconnect();
                return;
            }

        }

        Connect();

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        /*Debug.Log(roomList.Count);
        Debug.Log(PhotonNetwork.CountOfRooms);*/
        /*Debug.Log(roomList[0].MaxPlayers);
        Debug.Log(roomList[0].PlayerCount);
        Debug.Log(roomList[0].CustomProperties);*/
        UpdateCachedRoomList(roomList);
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            Debug.Log("�븮��Ʈ");
            Debug.Log(info);
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //������ ��ư ��Ȱ��ȭ
        /*EnterButton.interactable = false;
        //���� ���� ǥ��
        connectionInfoText.text = "���� ��õ� ��...";*/

        //������ �������� ������ �õ�
        if(GameManager.instance.multiState == "Multi")
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        
        //�ִ� 4���� ���� ������ �� �� ����
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2, IsVisible = true, CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "val", "0" } } });

    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            //JoinRoom("1001");
            JoinRoom();
        }
    }


    // Argument string roomNm ����
    public void JoinRoom()
    {
        //roomNum = roomNm;

        if (PhotonNetwork.NetworkClientState != ClientState.ConnectedToMasterServer && PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            //PhotonNetwork.ConnectUsingSettings();
            Invoke("JoinRoom", 2f);
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
            //PhotonNetwork.JoinOrCreateRoom(roomNm, new RoomOptions { MaxPlayers = 4, IsVisible = true, CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "val", "0" } } }, new TypedLobby(roomNm, LobbyType.Default));

        }
    }
    // Room�� ���� �õ��ߴµ� Master������ ����ȵ��� �� ȣ��
    /*public void JoinRoom()
    {
        if (PhotonNetwork.NetworkClientState != ClientState.ConnectedToMasterServer && PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            Invoke("JoinRoom", 2f);
        }
        else
        {

            PhotonNetwork.JoinOrCreateRoom(roomNum, new RoomOptions { MaxPlayers = 4, IsVisible = true, CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "val", "0" } } }, new TypedLobby(roomNum, LobbyType.Default));
        }
    }*/

    public override void OnJoinedRoom()
    {
        //���� ���� ǥ��

        //��� �� �����ڰ� Main ���� �ε��ϰ� ��
        /*if (roomNum.Contains("100"))
        {
            PhotonNetwork.LoadLevel("AuditoriumScene");
        }*/
        PhotonNetwork.LoadLevel("AuditoriumScene");
    }
}
