using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
using WebSocketSharp;
using System.Collections.Concurrent;
#endif

public class Chat : MonoBehaviour
{


    GameObject chatView;
    TMP_InputField chatField;
    GameObject chatContent;

    GameObject userListView;
    GameObject userListContent;

    PlayerState state;

    string imgFilePath;

    public GameObject chatListObj;
    public GameObject imgChatListObj;

    public GameObject userListObj;

    public Button sendBtn;
    public Button sendImgBtn;

    // ���� id ����
    string sessionId;


    [Serializable]
    class ChatData
    {
        public string type;                     // data Ÿ��
        public string sender;                   // ������ ���
        public string receiver;                 // �޴� ���
        public string message;                  // �޼���
        public string filePath;                 // �̹��� ���� ���
        public bool isMute;                     // ���Ұ� On/Off
        public string sessionId;                // ���� id

    }



#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void Connect();

    [DllImport("__Internal")]
    private static extern void SendMsg(string msg);

    [DllImport("__Internal")]
    private static extern void ImgFileOpen();
#endif
    [DllImport("__Internal")]
    private static extern void ImgFileSubmit();


    private void Awake()
    {
        chatView = gameObject.transform.Find("ChatView").gameObject;
        chatField = gameObject.transform.Find("ChatView").Find("MsgField").GetComponent<TMP_InputField>();
        chatContent = chatView.transform.Find("Scroll View").Find("Viewport").Find("Content").gameObject;

        chatListObj = Resources.Load<GameObject>("Chat\\ChatList");
        imgChatListObj = Resources.Load<GameObject>("Chat\\ImgChat");

        userListView = gameObject.transform.Find("UserListView").gameObject;
        userListContent = userListView.transform.Find("Scroll View").Find("Viewport").Find("Content").gameObject;

        userListObj = Resources.Load<GameObject>("UserList\\UserList");

        sendBtn = chatView.transform.Find("SendBtn").GetComponent<Button>();
        sendImgBtn = chatView.transform.Find("SendImgBtn").GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Connect();
        InitChat();
    }

    // Update is called once per frame
    void Update()
    {
        // ä�� ���� �Է� ó��
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {

            if (chatView.activeSelf && sendBtn.IsInteractable())
            {
                SendChatData("msg");
            }
        }
#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
        // Work the dispatched actions on the Unity main thread
        while (_actions.Count > 0)
        {
            if (_actions.TryDequeue(out var action))
            {
                action();
            }
        }
#endif

    }




#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
    private WebSocket ws;//���� ����

    // ����Ƽ �����Ϳ��� ���꽺���忡�� Resources.Load �Լ��� ������� �ʾ� ���ν����忡�� ����ǰ� �Ʒ� ���� ����.
    private readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();


    // ������ ����
    void Connect()
    {

        ws = new WebSocket("ws://192.168.1.113:8080/socket");
        ws.OnMessage += InitChatCheck; //�������� ����Ƽ ������ �޼����� �� ��� ������ �Լ��� ����Ѵ�.
        
        
        ws.OnOpen += ws_OnOpen;//������ ����� ��� ������ �Լ��� ����Ѵ�
        ws.OnClose += ws_OnClose;//������ ���� ��� ������ �Լ��� ����Ѵ�.


        ws.Connect();//������ �����Ѵ�.




    }

    // ������ ������� �� ����Ǵ� �Լ�
    void ws_OnOpen(object sender, System.EventArgs e)
    {
        Debug.Log("open"); //����� �ֿܼ� "open"�̶�� ��´�.

        //MasterCheck();
    }

    void ws_OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("close"); //����� �ֿܼ� "close"�̶�� ��´�.

        //QuitUserInfo();


    }



    public void InitChatCheck(object sender, MessageEventArgs e)
    {
        _actions.Enqueue(() => ChatCheck(sender,e));


        
    }

    public void ImgFileOpen()
    {
        Debug.Log("WebGL ����ÿ��� �̿밡��.");
    }



    // �������� ���� �޼����� �޾��� �� ����Ǵ� �Լ�
    public void ChatCheck(object sender, MessageEventArgs e)
    {

        ChatData chatData = JsonUtility.FromJson<ChatData>(e.Data);


        Debug.Log("unity : " + e.Data);
        if (chatData.type == "msg")
        {
            GameObject chatObj = Instantiate(chatListObj);
            chatObj.GetComponent<TextMeshProUGUI>().text = chatData.sender + " : " + chatData.message;
            chatObj.transform.SetParent(chatContent.transform);
            chatObj.transform.localScale = new Vector3(1, 1, 1);

        }
        else if (chatData.type == "sendFile")
        {
            Debug.Log("Ÿ�� : " + chatData.type + "  ������� : " + chatData.sender + "  msg : " + chatData.filePath);

            // ���� ����� ä��â�� ���
            GameObject chatObj = Instantiate(chatListObj);
            chatObj.GetComponent<TextMeshProUGUI>().text = chatData.sender + " : ";
            chatObj.transform.SetParent(chatContent.transform);
            chatObj.transform.localScale = new Vector3(1, 1, 1);

            // ���� �̹��� ä��â�� ���
            GameObject imgChatObj = Instantiate(imgChatListObj);
            imgChatObj.transform.SetParent(chatContent.transform);
            StartCoroutine(SetChatImg(chatData.filePath, imgChatObj));
        }
        else if (chatData.type == "master")
        {
            if (GameManager.instance.nickNm != "������")
            {
                SendChatData("sendUsrInfo");
            }
            
        }
        else if (chatData.type == "createUsr")
        {

            if (chatData.sender == "������")
            {
                Debug.Log("�������� ����");
                return;
            }

            if (GameManager.instance.nickNm != "������")
            {
                GameManager.instance.nickNm = chatData.sender;

                if (GameManager.instance.playerPrefab == null)
                {
                    GameManager.instance.CreatePlayer();
                }

                SendChatData("sendUsrInfo");
            }

            
        }
        else if(chatData.type == "sendUsrInfo")
        {
            if(chatData.sender == "������")
            {
                Debug.Log("�������� ����");
                return;
            }

            if(GameManager.instance.nickNm == "������")
            {
                Debug.Log("���� ����");
                Debug.Log(chatData.sender);
                GameObject chatObj = Instantiate(userListObj);
                chatObj.GetComponent<TextMeshProUGUI>().text = chatData.sender;
                chatObj.transform.SetParent(userListContent.transform);
                chatObj.transform.localScale = new Vector3(1, 1, 1);
            }
            /*else
            {
                GameManager.instance.nickNm = chatData.sender;

                if (GameManager.instance.playerPrefab == null)
                {
                    GameManager.instance.CreatePlayer();
                }

            }*/

            
        }
        else if(chatData.type == "muteUser")
        {
            Debug.Log(chatData.isMute);
            if(chatData.receiver == GameManager.instance.nickNm)
            {
                if (chatData.isMute)
                {
                    sendBtn.interactable = true;
                    sendImgBtn.interactable = true;
                    
                }
                else
                {
                    sendBtn.interactable = false;
                    sendImgBtn.interactable = false;
                }
            }
        }
        else if(chatData.type == "quitUserInfo")
        {
            if (GameManager.instance.nickNm == "������")
            {
                Debug.Log("���� ����");
                Debug.Log(chatData.sender);
                userListContent = userListView.transform.Find("Scroll View").Find("Viewport").Find("Content").gameObject;
                
                TextMeshProUGUI[] childList = userListContent.GetComponentsInChildren<TextMeshProUGUI>();

                if (childList.Length > 0)
                {
                    for (int i = 0; i < childList.Length; i++)
                    {
                        if (childList[i].text == chatData.sender)
                        {
                            Destroy(childList[i].gameObject);
                            return;
                        }
                    }
                }
            }
        }
        else if(chatData.type == "sessionId")
        {
            sessionId = chatData.sessionId;
            MasterCheck();
        }

        

    }

#elif UNITY_WEBGL && !UNITY_EDITOR

    public void ChatCheck(string msg)
    {
        ChatData chatData = JsonUtility.FromJson<ChatData>(msg);

        if(chatData.type == "msg"){
            GameObject chatObj = Instantiate(chatListObj);
            chatObj.GetComponent<TextMeshProUGUI>().text = chatData.sender + " : "+ chatData.message;
            chatObj.transform.SetParent(chatContent.transform);
            chatObj.transform.localScale = new Vector3(1, 1, 1);
            Debug.Log("unity web : "+msg);
        
        }else if(chatData.type == "sendFile"){
            Debug.Log("Ÿ�� : "+chatData.type + "  ������� : " + chatData.sender + "  msg : " + chatData.filePath);

            // ���� ����� ä��â�� ���
            GameObject chatObj = Instantiate(chatListObj);
            chatObj.GetComponent<TextMeshProUGUI>().text = chatData.sender + " : ";
            chatObj.transform.SetParent(chatContent.transform);
            chatObj.transform.localScale = new Vector3(1, 1, 1);

            // ���� �̹��� ä��â�� ���
            GameObject imgChatObj = Instantiate(imgChatListObj);
            imgChatObj.transform.SetParent(chatContent.transform);
            StartCoroutine(SetChatImg(chatData.filePath, imgChatObj));
        }
        else if (chatData.type == "master")
        {
            SendChatData("sendUsrInfo");
        }
        else if (chatData.type == "createUsr")
        {

            if (chatData.sender == "������")
            {
                Debug.Log("�������� ����");
                return;
            }

            if (GameManager.instance.nickNm != "������")
            {
                GameManager.instance.nickNm = chatData.sender;

                if (GameManager.instance.playerPrefab == null)
                {
                    GameManager.instance.CreatePlayer();
                }

                SendChatData("sendUsrInfo");
            }

            
        }
        else if(chatData.type == "sendUsrInfo")
        {
            if(chatData.sender == "������")
            {
                Debug.Log("�������� ����");
                return;
            }

            if(GameManager.instance.nickNm == "������")
            {
                Debug.Log("���� ����");
                Debug.Log(chatData.sender);
                GameObject chatObj = Instantiate(userListObj);
                chatObj.GetComponent<TextMeshProUGUI>().text = chatData.sender;
                chatObj.transform.SetParent(userListContent.transform);
                chatObj.transform.localScale = new Vector3(1, 1, 1);
            }
            /*else
            {
                GameManager.instance.nickNm = chatData.sender;

                if (GameManager.instance.playerPrefab == null)
                {
                    GameManager.instance.CreatePlayer();
                }

            }*/

            
        }
        else if(chatData.type == "muteUser")
        {
            Debug.Log(chatData.isMute);
            if(chatData.receiver == GameManager.instance.nickNm)
            {
                if (chatData.isMute)
                {
                    sendBtn.interactable = true;
                    sendImgBtn.interactable = true;
                    
                }
                else
                {
                    sendBtn.interactable = false;
                    sendImgBtn.interactable = false;
                }
            }
        }
        else if(chatData.type == "quitUserInfo")
        {
            if (GameManager.instance.nickNm == "������")
            {
                Debug.Log("���� ����");
                Debug.Log(chatData.sender);

                TextMeshProUGUI[] childList = userListContent.GetComponentsInChildren<TextMeshProUGUI>();

                if (childList != null)
                {
                    for (int i = 0; i < childList.Length; i++)
                    {
                        if(childList[i].text == chatData.sender)
                        {
                            Destroy(childList[i].gameObject);
                            return;
                        }
                    }
                }
            }
        }
        else if(chatData.type == "sessionId")
        {
            sessionId = chatData.sessionId;
            MasterCheck();
        }

        
    }

#endif

    //�������� ���� �޼��� ���� �Լ�
    public void SendChatData(string type)
    {
        ChatData chatData = new ChatData();
        chatData.sender = GameManager.instance.nickNm;
        chatData.type = type;
        if (type == "msg")
        {
            string msg = chatField.text;

            if (msg == "")
            {
                return;
            }

            Debug.Log("���� �޼��� : " + msg);

            chatField.text = "";
            chatData.message = msg;

            /*GameObject chatObj = Instantiate(chatListObj);
            chatObj.GetComponent<TextMeshProUGUI>().text = chatData.sender + " : " + chatData.message;
            chatObj.transform.SetParent(chatContent.transform);
            chatObj.transform.localScale = new Vector3(1, 1, 1);*/
        }
        else if (type == "file")
        {
            ImgFileSubmit();
        }
        else if (type == "sendFile")
        {

            string[] splitMsg = imgFilePath.Split('\\');

            Debug.Log("message : " + splitMsg[splitMsg.Length - 1]);
            Debug.Log("imgFilePath : " + imgFilePath);
            chatData.filePath = splitMsg[splitMsg.Length - 1];

        }
        else if (type == "master")
        {
            Debug.Log("master!!");

            if (GameManager.instance.playerPrefab == null)
            {
                GameManager.instance.CreatePlayer();
            }

        }
        else if (type == "createUsr")
        {
            Debug.Log("createUsr!!");
            chatData.sessionId = sessionId;
            chatData.sender = GameManager.instance.originNickNm;
        }
        else if (type == "sendUsrInfo")
        {
            Debug.Log("sendUsrInfo!!");
            //chatData.sessionId = sessionId;
            //chatData.sender = GameManager.instance.nickNm;
        }
        else if ( type == "quitUserInfo")
        {
            Debug.Log("quitUserInfo!!");
        }
#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
        ws.Send(JsonUtility.ToJson(chatData));//�������� �޼��� ����
#elif UNITY_WEBGL && !UNITY_EDITOR
        SendMsg(JsonUtility.ToJson(chatData));
#endif
    }



    public void MuteUser(string user, bool isMute)
    {
        ChatData chatData = new ChatData();
        chatData.type = "muteUser";
        chatData.sender = GameManager.instance.nickNm;
        chatData.receiver = user;
        chatData.isMute = isMute;

#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
        ws.Send(JsonUtility.ToJson(chatData));//�������� �޼��� ����
#elif UNITY_WEBGL && !UNITY_EDITOR
        SendMsg(JsonUtility.ToJson(chatData));
#endif

    }


    public void ReceiveImgFilePath(string filePath)
    {
        imgFilePath = filePath;
        Debug.Log(filePath);
        SendChatData("sendFile");
    }

    public void SendImgMsg(string fileNm)
    {
        SendChatData("file");
    }


    // ä��â ���� �̺�Ʈ
    public void OnOffChatView()
    {

        if (!userListView.activeSelf)
        {
            if (chatView.activeSelf)
            {
                GameManager.instance.playerState = state;
            }
            else
            {
                state = GameManager.instance.playerState;
                GameManager.instance.playerState = PlayerState.chat;

            }
        }
        chatView.SetActive(!chatView.activeSelf);
    }

    // ��������Ʈâ ���� �̺�Ʈ
    public void OnOffUserListView()
    {
        if (!chatView.activeSelf)
        {
            if (userListView.activeSelf)
            {

                GameManager.instance.playerState = state;

            }
            else
            {
                state = GameManager.instance.playerState;
                GameManager.instance.playerState = PlayerState.chat;
            }
        }
        
        userListView.SetActive(!userListView.activeSelf);
    }


    

    // ������ üũ �̺�Ʈ
    public void MasterCheck()
    {
        if(GameManager.instance.nickNm == "������")
        {
            SendChatData("master");
            gameObject.transform.Find("UsrListBtn").gameObject.SetActive(true);
        }
        else
        {
            SendChatData("createUsr");
        }
    }

    // ������ üũ �̺�Ʈ
    /*public void QuitUserInfo()
    {
            SendChatData("quitUserInfo");
    }*/

    // ä��â�� �̹��� ��� �Լ�
    IEnumerator SetChatImg(string url, GameObject imgChatObj)
    {
        string fileUrl = "http://192.168.1.113:8080/imgDown?file_nm=" + url;

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(fileUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //imgChatObj.GetComponent<ChatImgEvent>().imgUrl = url;
                Texture2D urlImg = DownloadHandlerTexture.GetContent(www);
                Debug.Log("width : " + urlImg.width + "  height : " + urlImg.height);
                Rect rect = new Rect(0, 0, urlImg.width, urlImg.height);

                Debug.Log("SetChatImg :" + www);
                GameObject imgObj = imgChatObj.transform.Find("Img").gameObject;
                imgObj.GetComponent<Image>().sprite = Sprite.Create(urlImg, rect, new Vector2(0f, 0f));
                //imgObj.GetComponent<Image>().preserveAspect = true;
                Debug.Log("preserveAspect : " + imgObj.GetComponent<Image>().preserveAspect);
                imgChatObj.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    // ä��â �ʱ�ȭ �̺�Ʈ
    public void InitChat()
    {
        Transform[] childList = chatContent.GetComponentsInChildren<Transform>();

        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                Destroy(childList[i].gameObject);
            }
        }

    }
}
