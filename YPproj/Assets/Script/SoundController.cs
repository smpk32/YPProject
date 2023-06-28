using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public Toggle BGMIcon;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void CheckSound()
    {
        if (BGMIcon.isOn == true) {
            Camera.main.GetComponent<AudioSource>().mute = true;
        }
        else
        {
            Camera.main.GetComponent<AudioSource>().mute = false; 
        }
    }
}
