using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PortionBtn : MonoBehaviour
{
    // Start is called before the first frame update
    private Button btn;
    private bool isDelay = false;
    private int coolTime=5;
    public Image imgCoolTime;
    public TMP_Text txtCoolTime;
    public TMP_Text txtPortionNum;
    public TMP_Text txtMessage;
    [SerializeField] public int PotionNum ;
    private int maxPotionCount = 3;
    public float hp;
    void Start()
    {
        PotionNum =3;
        txtPortionNum.text = PotionNum.ToString();
        this.txtPortionNum.gameObject.SetActive(true);
        hp = Player.Instance.heart.CUR_HP;
    }

    // Update is called once per frame
    private IEnumerator WaitForCooltime()
    {
        float delta = this.coolTime;
        //UI에 text에 업데이트
        while (true)
        {
            delta -= Time.deltaTime;
            this.txtCoolTime.text = string.Format("{0}", (int)delta);
            
            //imgCoolTime의 fillAmount도 같이 갱신
            //0 ~ 1
            float fillAmount = 1- (delta / this.coolTime);
            this.imgCoolTime.fillAmount = fillAmount;
        
            if (delta <= 0)
            {
                this.isDelay = false;
                break;
            }
            yield return null;
        }
        
        //txtCoolTime 비활성화
        this.txtCoolTime.gameObject.SetActive(false);
        this.txtPortionNum.gameObject.SetActive(true);
        this.imgCoolTime.fillAmount = 1;
    }

    private IEnumerator WaitMessagetime()
    {
        txtMessage.gameObject.SetActive(true); 
        yield return new WaitForSeconds(1);
        txtMessage.gameObject.SetActive(false);
    }

    public void Portion_Use()
    {
        if (this.isDelay) return;

        if (this.PotionNum == 0)
        {
            StartCoroutine("WaitMessagetime");
            return;
        }

        PotionNum--;
        // 체력차는 거 구현
        Player.Instance.heart.Restore_CUR_HP(50);
        txtPortionNum.text = PotionNum.ToString();
        this.isDelay = true;
        //cooltime 이미지의 fillAmount = 0
        this.imgCoolTime.fillAmount = 0;
        //txtCoolTime 활성화
        this.txtCoolTime.gameObject.SetActive(true);
        // 쿨타임을 보여준다.
        this.txtCoolTime.text = string.Format("{0}", this.coolTime);
            
        //시간 재기(Update)  
        this.StartCoroutine(this.WaitForCooltime());
    }

    public void RestorePotion()
    {
        PotionNum = Math.Max(PotionNum, maxPotionCount);
        txtPortionNum.text = PotionNum.ToString();
    }
}
