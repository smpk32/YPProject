using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserMuteCtrl : MonoBehaviour
{

    Toggle muteToggle;
    Chat chatComponent;


    void Awake()
    {
        muteToggle = gameObject.transform.Find("muteToggle").GetComponent<Toggle>();
        chatComponent = GameObject.Find("MainCanvas").GetComponent<Chat>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnOffToggle()
    {
        Debug.Log("toggle is on : "+ muteToggle.isOn);
        chatComponent.MuteUser(gameObject.GetComponent<TextMeshProUGUI>().text, muteToggle.isOn);
    }
}
