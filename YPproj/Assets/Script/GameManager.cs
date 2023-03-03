using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance = null;
    public string selectedCharacter = "MultiManPlayer";

    public GameObject playerPrefab;
    public int viewID;

    public string multiState = "Single";
    public int placeState = 0;
    public string sitNm = "";

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

    private void Awake()
    {
        if (null == instance)
        { // 씬 시작될때 인스턴스 초기화, 씬을 넘어갈때도 유지되기위한 처리
            instance = this;
            playerPrefab = GameObject.Find("PlayerObj");
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // instance가, GameManager가 존재한다면 GameObject 제거
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatePlayer()
    {


        //Quaternion rotate = Quaternion.Euler(0, -180, 0);
        // 네트워크 상의 모든 클라이언트들에서 생성 실행
        // 단, 해당 게임 오브젝트의 주도권은, 생성 메서드를 직접 실행한 클라이언트에게 있음
        GameObject player = PhotonNetwork.Instantiate(selectedCharacter, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        //플레이어 프리팹 할당
        GameManager.instance.playerPrefab = player;
        GameManager.instance.viewID = player.GetComponent<PhotonView>().ViewID;

        Camera.main.transform.parent = player.transform.Find("CameraObj").transform;

        GameObject.Find("SpawnSpot").GetComponent<PlaceMove2>().SetPlayerPos(GameManager.instance.placeState);

        //photonView.RPC("SetName", RpcTarget.AllBuffered, GameManager.instance.viewID, GameManager.instance.userId);

    }

    public void SetPlayer()
    {
        Camera.main.transform.parent = playerPrefab.transform.Find("CameraObject").transform;


        GameObject.Find("DragPanel").GetComponent<Drag>().SetPlayer();
    }

    public void Disconnect()
    {

        PhotonNetwork.Disconnect();
        //Destroy(this.gameObject);
        SceneManager.LoadScene("MainScene");

    }

}
