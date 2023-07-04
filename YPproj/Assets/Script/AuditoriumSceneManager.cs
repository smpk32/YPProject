using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditoriumSceneManager : MonoBehaviour
{

    private void Awake()
    {
        GameObject.Find("MainCanvas").gameObject.transform.Find("LoadingImage").gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.playerPrefab = null;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
