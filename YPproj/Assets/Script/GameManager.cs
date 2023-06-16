using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviourPunCallbacks
{
    public string localURL = "http://192.168.0.56:8080";
    //public string devURL = "http://203.228.54.47";
    public string baseURL = "";

    public enum Urltype
    {
        local,
        dev
    }

    public Urltype urlType = Urltype.local;


    public static GameManager instance = null;

    //�÷��̾� ĳ���� �ڽ�
    public GameObject playerPrefab;
    //
    public int viewID;

    public string nickNm = "";
    public string originNickNm = "";
    public string multiNickNm = "";

    // ��Ƽ�÷��� üũ ����
    public string multiState = "Single";
    //ĳ���� ���� üũ
    public bool firstCheck;
    // ��� �̸�
    public string bdnm = "Main";
    //��
    public int floor = 1;

    //����� id
    public string inhbtntPranAtflId = "";

    // ���õ� ����� ����
    public string eventId = "";


    /*���� �÷��̾ �ִ� ���
     *  0 > ����û
     *  1 > ���ð�
     *  2 > �����Ѹ��� (�̱�)
     *  3 > �����Ѹ���(��Ƽ)
    */
    public int placeState = 0;

    public PlayerState playerState = PlayerState.setting;

    // �÷��̾ ���� �ڸ��̸���� ����
    public string sitNm = "";

    // 
    public string selectCharacter = "ManPlayer1";

    // ������ üũ
    public bool isMaster = false;


    public static GameManager Instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
            }
            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }
    private static GameManager m_instance; // �̱����� �Ҵ�� static ����

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern string GetBaseURL();
#endif

    private void Awake()
    {
        if (null == instance)
        { // �� ���۵ɶ� �ν��Ͻ� �ʱ�ȭ, ���� �Ѿ���� �����Ǳ����� ó��
            instance = this;         

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // instance��, GameManager�� �����Ѵٸ� GameObject ����
            Destroy(this.gameObject);
        }

                /*if(urlType == Urltype.local)
                {
                    baseURL = localURL;
                }
                else
                {
                    baseURL = devURL;
                }*/

#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
        GetBaseURL();
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateSingleChacracter()
    {

        GameObject player = Instantiate(Resources.Load<GameObject>("CharacterModel\\" + selectCharacter));

        playerPrefab = player;
        player.transform.position = GameObject.Find("SpawnSpot").transform.Find("spawn1").transform.position;


        Camera.main.transform.parent = player.transform.Find("CameraObj").transform;
        player.transform.Find("CameraObj").transform.parent.name = "PlayerObj";

        player.GetComponent<SetPlayerNm>().SetNickNm();

        GameObject.Find("MainCanvas").transform.Find("DragPanel").transform.gameObject.SetActive(true);

        GameObject.Find("SpawnSpot").GetComponent<PlaceMove>().MapChange(GameManager.instance.placeState);

        playerState = PlayerState.normal;

        GameObject.Find("MainCanvas").transform.Find("DragPanel").GetComponent<CameraRotateController>().pc = playerPrefab.transform.Find("Player").GetComponent<PlayerController>();

    }

    // ��Ƽ�÷��̸���� �� �÷��̾� ������Ʈ �����ϴ� �Լ�
    public void CreatePlayer()
    {


        //Quaternion rotate = Quaternion.Euler(0, -180, 0);
        // ��Ʈ��ũ ���� ��� Ŭ���̾�Ʈ�鿡�� ���� ����
        // ��, �ش� ���� ������Ʈ�� �ֵ�����, ���� �޼��带 ���� ������ Ŭ���̾�Ʈ���� ����
        GameObject player = PhotonNetwork.Instantiate("CharacterModel\\" + selectCharacter, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        //�÷��̾� ������ �Ҵ�
        playerPrefab = player;


        Camera.main.transform.parent = player.transform.Find("CameraObj").transform;

        GameObject.Find("SpawnSpot").GetComponent<PlaceMove>().MapChange(GameManager.instance.placeState);

        GameManager.instance.viewID = player.GetComponent<PhotonView>().ViewID;
        player.GetComponent<SetPlayerNm>().SetNickNmPhotonAll();

        GameObject.Find("MainCanvas").transform.Find("DragPanel").GetComponent<CameraRotateController>().pc = playerPrefab.transform.Find("Player").GetComponent<PlayerController>();


    }

    // �÷��̾� ī�޶� ���� �Լ�
    public void SetPlayer()
    {
        Camera.main.transform.parent = playerPrefab.transform.Find("CameraObject").transform;

        GameObject.Find("DragPanel").GetComponent<Drag>().SetPlayer();
    }

    public void SetState(string state)
    {
        switch (state)
        {
            case "setting":
                playerState = PlayerState.setting;
                break;
            case "normal":
                playerState = PlayerState.normal;
                break;
            case "sitting":
                playerState = PlayerState.sitting;
                break;
            case "nav":
                playerState = PlayerState.nav;
                break;
            case "chat":
                playerState = PlayerState.chat;
                break;
        }
    }

    public void InitMaster(string masterChk)
    {
        isMaster = Convert.ToBoolean(masterChk);
    }


    // ��Ƽ >> �̱۷� �̵� �� Photon������� �Լ�
    public void Disconnect()
    {

        PhotonNetwork.Disconnect();
        //Destroy(this.gameObject);
        SceneManager.LoadScene("MainScene");

    }
    //��������
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetBaseUrl(string url)
    {
        baseURL = url;
    }

#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
    public void GetBaseURL()
    {
        baseURL = localURL;
    }
#endif

}
