using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEvent : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void ChangeFullScreen();
    
#endif

    public void ChangeScreenSize()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        ChangeFullScreen();
#endif
    }
}
