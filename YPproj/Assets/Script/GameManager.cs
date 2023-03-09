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

    // ��Ƽ�÷��� üũ ����
    public string multiState = "Single";

    /*���� �÷��̾ �ִ� ���
     *  0 > ����ȸ 1
     *  1 > ����ȸ 2
     *  2 > �����Ѹ���
     *  3 > �����Ѹ���(��Ƽ)
    */
    public int placeState = 1;

    // �÷��̾ ���� �ڸ��̸���� ����
    public string sitNm = "";

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

    private void Awake()
    {
        if (null == instance)
        { // �� ���۵ɶ� �ν��Ͻ� �ʱ�ȭ, ���� �Ѿ���� �����Ǳ����� ó��
            instance = this;
            playerPrefab = GameObject.Find("PlayerObj");
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // instance��, GameManager�� �����Ѵٸ� GameObject ����
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

    // ��Ƽ�÷��̸���� �� �÷��̾� �������� �����ϴ� �Լ�
    public void CreatePlayer()
    {


        //Quaternion rotate = Quaternion.Euler(0, -180, 0);
        // ��Ʈ��ũ ���� ��� Ŭ���̾�Ʈ�鿡�� ���� ����
        // ��, �ش� ���� ������Ʈ�� �ֵ�����, ���� �޼��带 ���� ������ Ŭ���̾�Ʈ���� ����
        GameObject player = PhotonNetwork.Instantiate(selectedCharacter, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        //�÷��̾� ������ �Ҵ�
        GameManager.instance.playerPrefab = player;
        GameManager.instance.viewID = player.GetComponent<PhotonView>().ViewID;

        Camera.main.transform.parent = player.transform.Find("CameraObj").transform;

        GameObject.Find("SpawnSpot").GetComponent<PlaceMove2>().SetPlayerPos(GameManager.instance.placeState);

        //photonView.RPC("SetName", RpcTarget.AllBuffered, GameManager.instance.viewID, GameManager.instance.userId);

    }

    // �÷��̾� ī�޶� ���� �Լ�
    public void SetPlayer()
    {
        Camera.main.transform.parent = playerPrefab.transform.Find("CameraObject").transform;


        GameObject.Find("DragPanel").GetComponent<Drag>().SetPlayer();
    }


    // ��Ƽ >> �̱۷� �̵� �� Photon������� �Լ�
    public void Disconnect()
    {

        PhotonNetwork.Disconnect();
        //Destroy(this.gameObject);
        SceneManager.LoadScene("MainScene");

    }

}
