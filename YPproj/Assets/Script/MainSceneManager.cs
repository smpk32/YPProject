using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviourPunCallbacks
{

    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enter()
    {

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
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

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

        //������ �������� ������ �õ�
        if(GameManager.instance.multiState == "Multi")
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        
        //�ִ� 4���� ���� ������ �� �� ����
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 8, IsVisible = true, CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "val", "0" } } });

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

        if (PhotonNetwork.NetworkClientState != ClientState.ConnectedToMasterServer && PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            //PhotonNetwork.ConnectUsingSettings();
            Invoke("JoinRoom", 2f);
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();

        }
    }

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
