using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhbtntInfo : MonoBehaviour
{
    public struct InhbtntData
    {
        public string inhbtnt_pran_id;              // 주민설명회 id
        public string inhbtnt_pran_nm;              // 주민설명회 명
        public string inhbtnt_pran_dc;              // 주민설명회 설명
        public string inhbtnt_pran_bgng_dt;         // 주민설명회 시작일
        public string inhbtnt_pran_end_dt;          // 주민설명회 종료일
        public string inhbtnt_pran_mvp_url;         // 주민설명회 영상 url
        public string use_yn;                       // 주민설명회 표출 유무
        public string progress;
        public InhbtntData(string _inhbtnt_pran_id,           
            string _inhbtnt_pran_nm,
            string _inhbtnt_pran_dc,
            string _inhbtnt_pran_bgng_dt,
            string _inhbtnt_pran_end_dt,
            string _inhbtnt_pran_mvp_url,
            string _use_yn,
            string _progress
            )
        {
            this.inhbtnt_pran_id = _inhbtnt_pran_id;              // 주민설명회 id
            this.inhbtnt_pran_nm = _inhbtnt_pran_nm;              // 주민설명회 명
            this.inhbtnt_pran_dc = _inhbtnt_pran_dc;              // 주민설명회 설명
            this.inhbtnt_pran_bgng_dt = _inhbtnt_pran_bgng_dt;         // 주민설명회 시작일
            this.inhbtnt_pran_end_dt = _inhbtnt_pran_end_dt;          // 주민설명회 종료일
            this.inhbtnt_pran_mvp_url = _inhbtnt_pran_mvp_url;         // 주민설명회 영상 url
            this.use_yn = _use_yn;                       // 주민설명회 표출 유무
            this.progress = _progress;
        }
    }

    public InhbtntData inhbtntdtlData = new InhbtntData(null, null, null, null, null, null, null, null);


    public void ClickInhbBtn()
    {
        Debug.Log("a =" + inhbtntdtlData.inhbtnt_pran_id);

        GameManager.instance.PresentationID = inhbtntdtlData.inhbtnt_pran_id;

        if (inhbtntdtlData.progress == "Y")
        {
            GameObject.Find("SpawnSpot").GetComponent<PlaceMove>().MapChange(3);
        }
        else
        {

            GameObject.Find("SpawnSpot").GetComponent<PlaceMove>().MapChange(2);
        }

        


    }
}
