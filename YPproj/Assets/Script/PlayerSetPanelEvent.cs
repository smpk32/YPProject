using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetPanelEvent : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

        // ����� ID �Է� �� ����Ű �Է����� ��
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            if (gameObject.activeSelf)
            {
                GameManager.instance.playerPrefab.GetComponent<SetPlayerNm>().SetNickNm();
            }
        }
    }
}
