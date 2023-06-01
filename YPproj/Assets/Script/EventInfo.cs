using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInfo : MonoBehaviour
{

    public struct EventDtlInfo
    {
        public string event_id;                 // 행사 id
        public string event_nm;                 // 행사 명
        public string event_image_atfl_id;      // 행사 포스터 이미지 파일 id
        public string event_dc;                 // 행사 설명
        public string event_bgng_dt;            // 행사 시작 일자
        public string event_end_dt;             // 행사 종료 일자
        public string event_place;              // 행사 장소
        public string event_hmpg_url;           // 행사 페이지 url

        public EventDtlInfo(string _event_id, string _event_nm, string _event_image_atfl_id, string _event_dc, string _event_bgng_dt, string _event_end_dt, string _event_place, string _event_hmpg_url)
        {
            this.event_id = _event_id;                 // 행사 id
            this.event_nm = _event_nm;                 // 행사 명
            this.event_image_atfl_id = _event_image_atfl_id;      // 행사 포스터 이미지 파일 id
            this.event_dc = _event_dc;                 // 행사 설명
            this.event_bgng_dt = _event_bgng_dt;            // 행사 시작 일자
            this.event_end_dt = _event_end_dt;             // 행사 종료 일자
            this.event_place = _event_place;              // 행사 장소
            this.event_hmpg_url = _event_hmpg_url;           // 행사 페이지 url
        }
    }


    public EventDtlInfo eventDtlInfo = new EventDtlInfo(null, null, null, null, null, null, null, null);




    public void ClickMoveButton()
    {
        GameObject.Find("SpawnSpot").GetComponent<PlaceMove>().MapChange(1);
    }
}
