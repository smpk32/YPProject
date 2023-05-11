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
    // 본관 / 별관 구분자
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {


            if (setPlace == "Sub")
            {
                GameManager.Instance.bdnm = "Sub";
            }
            else if (setPlace == "Main")
            {
                GameManager.Instance.bdnm = "Main";
            }


        }

       
    }

    


}
