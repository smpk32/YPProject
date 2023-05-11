using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SeatLayout : MonoBehaviour
{
    //VolumetricLines.VolumetricLineBehavior[] vtLines;

    public string imageNm;



#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void ShowSeatPanel(string panelId);
    
#endif


    // Start is called before the first frame update
    void Start()
    {
        //vtLines = gameObject.GetComponentsInChildren<VolumetricLines.VolumetricLineBehavior>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*public IEnumerator FadeSignPost()
    {
        float time = 0f;
        while (time < 1f)
        {
            foreach (VolumetricLines.VolumetricLineBehavior line in vtLines)
            {
                line.LineWidth = time*10/2;
            }

            time += Time.deltaTime;
            yield return null;
        }
        foreach (VolumetricLines.VolumetricLineBehavior line in vtLines)
        {
            line.LineWidth = 5;
        }

        
    }

    public IEnumerator ShowSignPost()
    {
        float time = 0f;
        while (time < 1f)
        {
            foreach (VolumetricLines.VolumetricLineBehavior line in vtLines)
            {
                line.LineWidth = 5-(time * 10/2);

            }

            time += Time.deltaTime;
            yield return null;
        }
        foreach (VolumetricLines.VolumetricLineBehavior line in vtLines)
        {
            line.LineWidth = 0;
        }

    }

    private void OnMouseEnter()
    {
        foreach (VolumetricLines.VolumetricLineBehavior line in vtLines)
        {
            line.LineWidth = 10;
            line.LineColor = new Color(1.000f, 1.000f, 1.000f, 1.000f);
        }

    }

    private void OnMouseExit()
    {
        foreach (VolumetricLines.VolumetricLineBehavior line in vtLines)
        {
            line.LineWidth = 10;
            //line.LineColor = new Color(0.000f, 1.000f, 1.000f, 1.000f);
            line.LineColor = new Color(0.000f, 0.000f, 0.000f, 0.000f);
        }
        //StartCoroutine("ShowSignPost");
    }*/

    public void ShowLayout()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        ShowSeatPanel(imageNm);
        GameManager.instance.SetState("setting");
#endif

    }
}
