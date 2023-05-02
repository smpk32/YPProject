using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    RaycastHit hit;
    Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ClickNPC();
    }

    public void ClickNPC()
    {
      

        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "NPC")
                {
                    GameObject.Find("MainCanvas").transform.Find("NPCPanel").transform.gameObject.SetActive(true);

                }
            }
        }
    }

    public void CloseNPC()
    {
        GameObject.Find("MainCanvas").transform.Find("NPCPanel").transform.gameObject.SetActive(false);
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }


}
