using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRoll : MonoBehaviour
{
    private int coolTime=5;
    public Image imgCoolTime;
    public Image backImg;
    public TMP_Text txtCoolTime;
    public bool isDelay = false;


    public void Init()
    {
        this.backImg.gameObject.SetActive(false);
        //txtCoolTime 활성화
        this.txtCoolTime.gameObject.SetActive(false);
        //cooltime 이미지의 fillAmount = 0
        this.imgCoolTime.fillAmount = 0;
    }
    
    public void RollSkillCool()
    {
            this.isDelay = true;
            this.backImg.gameObject.SetActive(true);
            this.txtCoolTime.gameObject.SetActive(true);
            this.imgCoolTime.fillAmount = 0;
            //txtCoolTime 활성화
            this.txtCoolTime.gameObject.SetActive(true);
            // 쿨타임을 보여준다.
            this.txtCoolTime.text = string.Format("{0}", this.coolTime);
            
            //시간 재기(Update)  
            this.StartCoroutine(this.WaitForRolltime());
    }
    
    private IEnumerator WaitForRolltime()
    {
        float rollDelta = Player.Instance.dashCooltime;
        float roll = Player.Instance.dashCooltime;
        //UI에 text에 업데이트
        while (true)
        {
            rollDelta -= Time.deltaTime;
            this.txtCoolTime.text = string.Format("{0}", (int)rollDelta);
            
            //imgCoolTime의 fillAmount도 같이 갱신
            //0 ~ 1
            float fillAmount = 1- (rollDelta / roll);
            this.imgCoolTime.fillAmount = fillAmount;

            if (rollDelta <= 0)
            {
                this.isDelay = false;
                break;
            }
            yield return null;
        }
        
        //txtCoolTime 비활성화
        this.txtCoolTime.gameObject.SetActive(false);
        this.backImg.gameObject.SetActive(false);
        this.imgCoolTime.fillAmount = 0;
        
    }
}
