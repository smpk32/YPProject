using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{

    public string setPlace;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (setPlace == "Sub")
            {
                GameManager.Instance.bdnm = "Sub";
            }
            else
            {
                GameManager.Instance.bdnm = "Main";
            }
        }
    }
}
