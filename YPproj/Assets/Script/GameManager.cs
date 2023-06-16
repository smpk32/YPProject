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

    //플레이어 캐릭터 자신
    public GameObject playerPrefab;
    //
    public int viewID;

    public string nickNm = "";
    public string originNickNm = "";
    public string multiNickNm = "";

    // 멀티플레이 체크 변수
    public string multiState = "Single";
    //캐릭터 선택 체크
    public bool firstCheck;
    // 장소 이름
    public string bdnm = "Main";
    //층
    public int floor = 1;

    //강당씬 id
    public string inhbtntPranAtflId = "";

    // 선택된 행사장 제목
    public string eventId = "";


    /*현재 플레이어가 있는 장소
     *  0 > 양평군청
     *  1 > 전시관
     *  2 > 소통한마당 (싱글)
     *  3 > 소통한마당(멀티)
    */
    public int placeState = 0;

    public PlayerState playerState = PlayerState.setting;

    // 플레이어가 앉은 자리이름담는 변수
    public string sitNm = "";

    // 
    public string selectCharacter = "ManPlayer1";

    // 관리자 체크
    public bool isMaster = false;


    public static GameManager Instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }
            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern string GetBaseURL();
#endif

    private void Awake()
    {
        if (null == instance)
        { // 씬 시작될때 인스턴스 초기화, 씬을 넘어갈때도 유지되기위한 처리
            instance = this;         

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // instance가, GameManager가 존재한다면 GameObject 제거
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

    // 멀티플레이모드일 때 플레이어 오브젝트 생성하는 함수
    public void CreatePlayer()
    {


        //Quaternion rotate = Quaternion.Euler(0, -180, 0);
        // 네트워크 상의 모든 클라이언트들에서 생성 실행
        // 단, 해당 게임 오브젝트의 주도권은, 생성 메서드를 직접 실행한 클라이언트에게 있음
        GameObject player = PhotonNetwork.Instantiate("CharacterModel\\" + selectCharacter, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        //플레이어 프리팹 할당
        playerPrefab = player;


        Camera.main.transform.parent = player.transform.Find("CameraObj").transform;

        GameObject.Find("SpawnSpot").GetComponent<PlaceMove>().MapChange(GameManager.instance.placeState);

        GameManager.instance.viewID = player.GetComponent<PhotonView>().ViewID;
        player.GetComponent<SetPlayerNm>().SetNickNmPhotonAll();

        GameObject.Find("MainCanvas").transform.Find("DragPanel").GetComponent<CameraRotateController>().pc = playerPrefab.transform.Find("Player").GetComponent<PlayerController>();


    }

    // 플레이어 카메라 설정 함수
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


    // 멀티 >> 싱글로 이동 시 Photon연결끊는 함수
    public void Disconnect()
    {

        PhotonNetwork.Disconnect();
        //Destroy(this.gameObject);
        SceneManager.LoadScene("MainScene");

    }
    //게임종료
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
