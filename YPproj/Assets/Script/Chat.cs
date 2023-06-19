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
    public GameObject chatNmObj;
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
        public bool isMaster;                   // ������ üũ

    }



#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void ConnectStart();

    [DllImport("__Internal")]
    private static extern void SendMsg(string msg);

    [DllImport("__Internal")]
    private static extern void ImgFileOpen();

    [DllImport("__Internal")]
    private static extern void ConnectClose();

    
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
        chatNmObj = Resources.Load<GameObject>("Chat\\ChatNm");

        userListView = gameObject.transform.Find("UserListView").gameObject;
        userListContent = userListView.transform.Find("Scroll View").Find("Viewport").Find("Content").gameObject;

        userListObj = Resources.Load<GameObject>("UserList\\UserList");

        sendBtn = chatView.transform.Find("SendBtn").GetComponent<Button>();
        sendImgBtn = chatView.transform.Find("SendImgBtn").GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ConnectStart();
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
    void ConnectStart()
    {
        string socAddress = GameManager.instance.baseURL.Replace("http", "ws");
        ws = new WebSocket(socAddress+"/socket");
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

        // �翬�� �õ�
        //ConnectStart();
        InitChat();

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
        // �ٸ� ������ �޼��������� ��
        if (chatData.type == "msg")
        {
            GameObject chaNmTxtObj = Instantiate(chatNmObj);
            chaNmTxtObj.GetComponent<TextMeshProUGUI>().text = "\n" + chatData.sender + " �� ";
            if (chatData.isMaster)
            {
                chaNmTxtObj.GetComponent<TextMeshProUGUI>().color = new Color32(126,180,196,255);
            }
            chaNmTxtObj.transform.SetParent(chatContent.transform);
            chaNmTxtObj.transform.localScale = new Vector3(1, 1, 1);

            GameObject chatObj = Instantiate(chatListObj);
            chatObj.transform.Find("Text").GetComponent<TextMeshProUGUI>().text =  chatData.message;
            chatObj.transform.SetParent(chatContent.transform);
            chatObj.transform.localScale = new Vector3(1, 1, 1);

        }
        // �ٸ� ������ �̹��������� ��
        else if (chatData.type == "sendFile")
        {
            Debug.Log("Ÿ�� : " + chatData.type + "  ������� : " + chatData.sender + "  msg : " + chatData.filePath);

            // ���� ����� ä��â�� ���
            GameObject chaNmTxtObj = Instantiate(chatNmObj);
            chaNmTxtObj.GetComponent<TextMeshProUGUI>().text = "\n" + chatData.sender + " �� ";
            chaNmTxtObj.transform.SetParent(chatContent.transform);
            chaNmTxtObj.transform.localScale = new Vector3(1, 1, 1);

            // ���� �̹��� ä��â�� ���
            GameObject imgChatObj = Instantiate(imgChatListObj);
            imgChatObj.transform.SetParent(chatContent.transform);
            StartCoroutine(SetChatImg(chatData.filePath, imgChatObj));
        }
        // �����ڰ� ������ ��
        else if (chatData.type == "master")
        {

            if (!GameManager.instance.isMaster)
            {
                SendChatData("sendUsrInfo");
            }
            else
            {
                Debug.Log(sessionId);
                GameManager.instance.nickNm = chatData.sender;
                GameManager.instance.playerPrefab.GetComponent<SetPlayerNm>().SetNickNmPhotonAll();
            }
            
        }
        // ���� ���� �� ĳ���� ����
        else if (chatData.type == "createUsr")
        {

            if (chatData.isMaster)
            {
                return;
            }

            if (!GameManager.instance.isMaster)
            {
                GameManager.instance.nickNm = chatData.sender;

                if (GameManager.instance.playerPrefab == null)
                {
                    GameManager.instance.CreatePlayer();
                }

                SendChatData("sendUsrInfo");
            }

            
        }
        // �� ���� �����ڿ��� ����
        else if(chatData.type == "sendUsrInfo")
        {
            if(chatData.isMaster)
            {
                Debug.Log("�������� ����");
                return;
            }

            if(GameManager.instance.isMaster)
            {
                Debug.Log("���� ����");
                Debug.Log(chatData.sender);
                GameObject chatObj = Instantiate(userListObj);
                chatObj.GetComponent<TextMeshProUGUI>().text = chatData.sender;
                chatObj.transform.SetParent(userListContent.transform);
                chatObj.transform.localScale = new Vector3(1, 1, 1);
            }

            
        }
        // ���� ���Ұ�
        else if(chatData.type == "muteUser")
        {
            Debug.Log(chatData.isMute);
            if(chatData.receiver == GameManager.instance.nickNm)
            {
                if (chatData.isMute)
                {
                    chatField.text = "";
                    sendBtn.interactable = true;
                    sendImgBtn.interactable = true;
                    chatField.interactable = true;
                    chatField.placeholder.GetComponent<TextMeshProUGUI>().text = "�޼����� �Է��ϼ���.";
                    
                }
                else
                {
                    chatField.text = "ä�ñ��������Դϴ�.";
                    sendBtn.interactable = false;
                    sendImgBtn.interactable = false;
                    chatField.interactable = false;
                    chatField.placeholder.GetComponent<TextMeshProUGUI>().text = "ä�ñ��������Դϴ�.";
                }
            }
        }
        // ���� ������ ��
        else if(chatData.type == "quitUserInfo")
        {
            if (GameManager.instance.isMaster)
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
        // �ʱ� ���� �� session ID ����
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
            GameObject chaNmTxtObj = Instantiate(chatNmObj);
            chaNmTxtObj.GetComponent<TextMeshProUGUI>().text = "\n" + chatData.sender + " �� ";
            if (chatData.isMaster)
            {
                chaNmTxtObj.GetComponent<TextMeshProUGUI>().color = new Color32(126,180,196,255);
            }
            chaNmTxtObj.transform.SetParent(chatContent.transform);
            chaNmTxtObj.transform.localScale = new Vector3(1, 1, 1);

            GameObject chatObj = Instantiate(chatListObj);
            chatObj.transform.Find("Text").GetComponent<TextMeshProUGUI>().text =  chatData.message;
            chatObj.transform.SetParent(chatContent.transform);
            chatObj.transform.localScale = new Vector3(1, 1, 1);
        
        }else if(chatData.type == "sendFile"){
            Debug.Log("Ÿ�� : "+chatData.type + "  ������� : " + chatData.sender + "  msg : " + chatData.filePath);

            // ���� ����� ä��â�� ���
            GameObject chaNmTxtObj = Instantiate(chatNmObj);
            chaNmTxtObj.GetComponent<TextMeshProUGUI>().text = "\n" + chatData.sender + " �� ";
            chaNmTxtObj.transform.SetParent(chatContent.transform);
            chaNmTxtObj.transform.localScale = new Vector3(1, 1, 1);

            // ���� �̹��� ä��â�� ���
            GameObject imgChatObj = Instantiate(imgChatListObj);
            imgChatObj.transform.SetParent(chatContent.transform);
            StartCoroutine(SetChatImg(chatData.filePath, imgChatObj));
        }
        else if (chatData.type == "master")
        {
            if (!GameManager.instance.isMaster)
            {
                SendChatData("sendUsrInfo");
            }
            else
            {
                GameManager.instance.nickNm = chatData.sender;
                GameManager.instance.playerPrefab.GetComponent<SetPlayerNm>().SetNickNmPhotonAll();
            }
        }
        else if (chatData.type == "createUsr")
        {

            if (chatData.isMaster)
            {
                Debug.Log("�������� ����");
                return;
            }

            if (!GameManager.instance.isMaster)
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
            if(chatData.isMaster)
            {
                Debug.Log("�������� ����");
                return;
            }

            if(GameManager.instance.isMaster)
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
                    chatField.text = "";
                    sendBtn.interactable = true;
                    sendImgBtn.interactable = true;
                    chatField.interactable = true;
                    chatField.placeholder.GetComponent<TextMeshProUGUI>().text = "�޼����� �Է��ϼ���.";
                    
                }
                else
                {
                    chatField.text = "ä�ñ��������Դϴ�.";
                    sendBtn.interactable = false;
                    sendImgBtn.interactable = false;
                    chatField.interactable = false;
                    chatField.placeholder.GetComponent<TextMeshProUGUI>().text = "ä�ñ��������Դϴ�.";
                }
            }
        }
        else if(chatData.type == "quitUserInfo")
        {
            if (GameManager.instance.isMaster)
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
        chatData.isMaster = GameManager.instance.isMaster;

        // �޼��� ����
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

        // �̹��� ��������
        else if (type == "file")
        {
            ImgFileSubmit();
        }
        // �̹��� ���� ����
        else if (type == "sendFile")
        {

            string[] splitMsg = imgFilePath.Split('\\');

            Debug.Log("message : " + splitMsg[splitMsg.Length - 1]);
            Debug.Log("imgFilePath : " + imgFilePath);
            chatData.filePath = splitMsg[splitMsg.Length - 1];

        }
        // ������ ����
        else if (type == "master")
        {
            Debug.Log("master!!");
            chatData.sessionId = sessionId;
            if (GameManager.instance.playerPrefab == null)
            {
                GameManager.instance.CreatePlayer();
            }

        }
        // ���� ����
        else if (type == "createUsr")
        {
            Debug.Log("createUsr!!");
            chatData.sessionId = sessionId;
            chatData.sender = GameManager.instance.originNickNm;
        }
        // ���� ���� ����
        else if (type == "sendUsrInfo")
        {
            Debug.Log("sendUsrInfo!!");
            chatData.sessionId = sessionId;
            //chatData.sender = GameManager.instance.nickNm;
        }
        // ���� ������ ��
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
    public void CloseSocket()
    {
#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
        ws.Close();
#elif UNITY_WEBGL && !UNITY_EDITOR
        ConnectClose();
#endif
    }

    // ���� ���Ұ� �̺�Ʈ
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


    // �̹��� ���� ��� ����
    public void ReceiveImgFilePath(string filePath)
    {
        imgFilePath = filePath;
        SendChatData("sendFile");
    }

    // �̹��� ���� �̺�Ʈ
    public void SendImgMsg(string fileNm)
    {
        SendChatData("file");
    }


    // ä��â ���� �̺�Ʈ
    public void OnOffChatView()
    {
        chatView.SetActive(!chatView.activeSelf);
    }

    // ��������Ʈâ ���� �̺�Ʈ
    public void OnOffUserListView()
    {
        userListView.SetActive(!userListView.activeSelf);
    }


    

    // ������ üũ �̺�Ʈ
    public void MasterCheck()
    {
        if(GameManager.instance.isMaster)
        {
            SendChatData("master");
            gameObject.transform.Find("UsrListBtn").gameObject.SetActive(true);
        }
        else
        {
            SendChatData("createUsr");
        }
    }

    // ä��â�� �̹��� ��� �Լ�
    IEnumerator SetChatImg(string url, GameObject imgChatObj)
    {
        string fileUrl = GameManager.instance.baseURL+"/imgDown?file_nm=" + url;

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
                Rect rect = new Rect(0, 0, urlImg.width, urlImg.height);

                GameObject imgObj = imgChatObj.transform.Find("Img").gameObject;
                imgObj.GetComponent<Image>().sprite = Sprite.Create(urlImg, rect, new Vector2(0f, 0f));
                //imgObj.GetComponent<Image>().preserveAspect = true;
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

    public void CloseFrameDtlPanel()
    {
        gameObject.transform.Find("FrameDtlPanel").gameObject.SetActive(false);
    }
}
