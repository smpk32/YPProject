using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{

    RaycastHit hit;
    Ray ray;

    public TextMeshProUGUI dialogText;
    string sampleText;

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
      
        sampleText = "안녕하세요 이곳은 양평군청입니다. \n 토지 관련 안내는 저를 클릭하세요 ";
      
        StartCoroutine(Typing(sampleText));

    }

    // Update is called once per frame
    void Update()
    {
        //ClickNPC();
    }


    //CameraRotateController.OnPointerUp() 로 옮김
    public void ClickNPC()
    {
        GameObject.Find("MainCanvas").transform.Find("NPCPanel").transform.gameObject.SetActive(true);

        /*if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "NPC")
                {
                    

                }
            }
        }*/
    }

    public void CloseNPC()
    {
        GameObject.Find("MainCanvas").transform.Find("NPCPanel").transform.gameObject.SetActive(false);
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    IEnumerator Typing(string text)
    {
        gameObject.transform.Find("TextBox").transform.gameObject.SetActive(true);
        foreach (char letter in text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        gameObject.transform.Find("TextBox").transform.gameObject.SetActive(false);
        dialogText.text = "";

        yield return new WaitForSeconds(3f);
        StartCoroutine(Typing(sampleText));
    }
}
