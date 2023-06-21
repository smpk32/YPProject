using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventInfo : MonoBehaviour
{

    public struct EventDtlInfo
    {
        public string event_id;                 // ��� id
        public string event_nm;                 // ��� ��
        public string event_image_atfl_id;      // ��� ������ �̹��� ���� id
        public string event_dc;                 // ��� ����
        public string event_bgng_dt;            // ��� ���� ����
        public string event_end_dt;             // ��� ���� ����
        public string event_place;              // ��� ���
        public string event_hmpg_url;           // ��� ������ url
        public string progress;                 // ��� ���� ����

        public EventDtlInfo(string _event_id, string _event_nm, string _event_image_atfl_id, string _event_dc, string _event_bgng_dt, string _event_end_dt, string _event_place, string _event_hmpg_url, string _progress)
        {
            this.event_id = _event_id;                 // ��� id
            this.event_nm = _event_nm;                 // ��� ��
            this.event_image_atfl_id = _event_image_atfl_id;      // ��� ������ �̹��� ���� id
            this.event_dc = _event_dc;                 // ��� ����
            this.event_bgng_dt = _event_bgng_dt;            // ��� ���� ����
            this.event_end_dt = _event_end_dt;             // ��� ���� ����
            this.event_place = _event_place;              // ��� ���
            this.event_hmpg_url = _event_hmpg_url;           // ��� ������ url
            this.progress = _progress;           // ��� ������ url
        }
    }


    public EventDtlInfo eventDtlInfo = new EventDtlInfo(null, null, null, null, null, null, null, null, null);




    public void ClickMoveButton()
    {
        GameManager.instance.eventId = eventDtlInfo.event_id;
        GameManager.instance.eventFileId = eventDtlInfo.event_image_atfl_id;
        GameManager.instance.eventNm = eventDtlInfo.event_nm;
        GameManager.instance.eventDc = eventDtlInfo.event_dc;
        GameObject.Find("SpawnSpot").GetComponent<PlaceMove>().MapChange(1);
        
    }
}
