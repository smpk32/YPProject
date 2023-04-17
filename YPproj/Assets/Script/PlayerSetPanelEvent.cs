using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetPanelEvent : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

        // 사용자 ID 입력 후 엔터키 입력했을 때
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            if (gameObject.activeSelf)
            {
                GameManager.instance.playerPrefab.GetComponent<SetPlayerNm>().SetNickNm();
            }
        }
    }
}
