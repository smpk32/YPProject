using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgFilter : MonoBehaviour
{
    /*class WordData
    {
        public List<string> badWords;
        public List<string> changeWords;
    }

    WordData wordData = new WordData();*/

    public List<string> badWords = new List<string>();
    public List<string> changeWords = new List<string>();


    public List<string> chkStrings = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ReadTxt");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // 서버에서 txt파일 읽어오는 함수
    IEnumerator ReadTxt()
    {
        WWW wwww = new WWW("http://192.168.1.142:8080/badWords/badWordList.txt");
        yield return wwww;

        string[] lines = wwww.text.Split('\n');

        if(lines.Length != 0)
        {
            foreach (string line in lines)
            {
                badWords.Add(line);

                string changetext = new string('*', line.Length);
                /*for (int j = 0; j < line.Length; j++)
                {
                    changetext += "*";
                }*/
                changeWords.Add(changetext);

            }

        }
    }

    public string CheckBadWords(string msg)
    {
        string changedMsg = msg;

        if(badWords == null)
        {
            return null;
        }

        for(int i = 0; i < badWords.Count; i++)
        {
            if(changedMsg.Contains(badWords[i]))
            {
                changedMsg = changedMsg.Replace(badWords[i], changeWords[i]);
            }
        }

        return changedMsg;


        /*foreach(string badword in badWords)
        {
            if (changeMsg.Contains(badword))
            {
                changeMsg = changeMsg.Replace()
            }
        }*/
    }


    public void FilterTest()
    {
        string changedTxt;
        foreach (string chkString in chkStrings)
        {
            //Debug.Log("originTxt : " + chkString);
            changedTxt = chkString.Replace(" ","");
            Debug.Log("originTxt : " + changedTxt);
            Debug.Log("changedTxt : " + CheckBadWords(changedTxt));
        }
    }





}
