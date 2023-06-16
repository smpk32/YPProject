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
        public string inhbtnt_pran_atfl_id;         // 주민설명회 영상 url
        public string use_yn;                       // 주민설명회 표출 유무
        public string progress;
        public string stre_file_nm;
        public InhbtntData(string _inhbtnt_pran_id,           
            string _inhbtnt_pran_nm,
            string _inhbtnt_pran_dc,
            string _inhbtnt_pran_bgng_dt,
            string _inhbtnt_pran_end_dt,
            string _inhbtnt_pran_atfl_id,
            string _use_yn,
            string _progress,
            string _stre_file_nm
            )
        {
            this.inhbtnt_pran_id = _inhbtnt_pran_id;              // 주민설명회 id
            this.inhbtnt_pran_nm = _inhbtnt_pran_nm;              // 주민설명회 명
            this.inhbtnt_pran_dc = _inhbtnt_pran_dc;              // 주민설명회 설명
            this.inhbtnt_pran_bgng_dt = _inhbtnt_pran_bgng_dt;         // 주민설명회 시작일
            this.inhbtnt_pran_end_dt = _inhbtnt_pran_end_dt;          // 주민설명회 종료일
            this.inhbtnt_pran_atfl_id = _inhbtnt_pran_atfl_id;         // 주민설명회 영상 url
            this.use_yn = _use_yn;                       // 주민설명회 표출 유무
            this.progress = _progress;
            this.stre_file_nm = _stre_file_nm;
        }
    }

    public InhbtntData inhbtntdtlData = new InhbtntData(null, null, null, null, null, null, null, null,null);


    //주민설명회 버튼 클릭시 캐릭터 이동 및 전역 변수 id값 삽입
    public void ClickInhbBtn()
    {
      
        GameManager.instance.inhbtntPranAtflId = inhbtntdtlData.inhbtnt_pran_atfl_id;

        //(멀티)
        if (inhbtntdtlData.progress == "Y")
        {
            GameObject.Find("SpawnSpot").GetComponent<PlaceMove>().MapChange(3);
        }
        //(싱글)
        else
        {

            GameObject.Find("SpawnSpot").GetComponent<PlaceMove>().MapChange(2);
        }

        


    }
}
