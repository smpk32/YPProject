using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhbtntInfo : MonoBehaviour
{
    public struct InhbtntData
    {
        public string inhbtnt_pran_id;              // �ֹμ���ȸ id
        public string inhbtnt_pran_nm;              // �ֹμ���ȸ ��
        public string inhbtnt_pran_dc;              // �ֹμ���ȸ ����
        public string inhbtnt_pran_bgng_dt;         // �ֹμ���ȸ ������
        public string inhbtnt_pran_end_dt;          // �ֹμ���ȸ ������
        public string inhbtnt_pran_mvp_url;         // �ֹμ���ȸ ���� url
        public string use_yn;                       // �ֹμ���ȸ ǥ�� ����
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
            this.inhbtnt_pran_id = _inhbtnt_pran_id;              // �ֹμ���ȸ id
            this.inhbtnt_pran_nm = _inhbtnt_pran_nm;              // �ֹμ���ȸ ��
            this.inhbtnt_pran_dc = _inhbtnt_pran_dc;              // �ֹμ���ȸ ����
            this.inhbtnt_pran_bgng_dt = _inhbtnt_pran_bgng_dt;         // �ֹμ���ȸ ������
            this.inhbtnt_pran_end_dt = _inhbtnt_pran_end_dt;          // �ֹμ���ȸ ������
            this.inhbtnt_pran_mvp_url = _inhbtnt_pran_mvp_url;         // �ֹμ���ȸ ���� url
            this.use_yn = _use_yn;                       // �ֹμ���ȸ ǥ�� ����
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
