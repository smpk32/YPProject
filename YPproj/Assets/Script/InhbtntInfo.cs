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
        public string inhbtnt_pran_atfl_id;         // �ֹμ���ȸ ���� url
        public string use_yn;                       // �ֹμ���ȸ ǥ�� ����
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
            this.inhbtnt_pran_id = _inhbtnt_pran_id;              // �ֹμ���ȸ id
            this.inhbtnt_pran_nm = _inhbtnt_pran_nm;              // �ֹμ���ȸ ��
            this.inhbtnt_pran_dc = _inhbtnt_pran_dc;              // �ֹμ���ȸ ����
            this.inhbtnt_pran_bgng_dt = _inhbtnt_pran_bgng_dt;         // �ֹμ���ȸ ������
            this.inhbtnt_pran_end_dt = _inhbtnt_pran_end_dt;          // �ֹμ���ȸ ������
            this.inhbtnt_pran_atfl_id = _inhbtnt_pran_atfl_id;         // �ֹμ���ȸ ���� url
            this.use_yn = _use_yn;                       // �ֹμ���ȸ ǥ�� ����
            this.progress = _progress;
            this.stre_file_nm = _stre_file_nm;
        }
    }

    public InhbtntData inhbtntdtlData = new InhbtntData(null, null, null, null, null, null, null, null,null);


    //�ֹμ���ȸ ��ư Ŭ���� ĳ���� �̵� �� ���� ���� id�� ����
    public void ClickInhbBtn()
    {
      
        GameManager.instance.inhbtntPranAtflId = inhbtntdtlData.inhbtnt_pran_atfl_id;

        //(��Ƽ)
        if (inhbtntdtlData.progress == "Y")
        {
            GameObject.Find("SpawnSpot").GetComponent<PlaceMove>().MapChange(3);
        }
        //(�̱�)
        else
        {

            GameObject.Find("SpawnSpot").GetComponent<PlaceMove>().MapChange(2);
        }

        


    }
}
