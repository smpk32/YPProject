using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public EventDtlInfo(string _event_id, string _event_nm, string _event_image_atfl_id, string _event_dc, string _event_bgng_dt, string _event_end_dt, string _event_place, string _event_hmpg_url)
        {
            this.event_id = _event_id;                 // ��� id
            this.event_nm = _event_nm;                 // ��� ��
            this.event_image_atfl_id = _event_image_atfl_id;      // ��� ������ �̹��� ���� id
            this.event_dc = _event_dc;                 // ��� ����
            this.event_bgng_dt = _event_bgng_dt;            // ��� ���� ����
            this.event_end_dt = _event_end_dt;             // ��� ���� ����
            this.event_place = _event_place;              // ��� ���
            this.event_hmpg_url = _event_hmpg_url;           // ��� ������ url
        }
    }


    public EventDtlInfo eventDtlInfo = new EventDtlInfo(null, null, null, null, null, null, null, null);




    public void ClickMoveButton()
    {
        GameObject.Find("SpawnSpot").GetComponent<PlaceMove>().MapChange(1);
    }
}
