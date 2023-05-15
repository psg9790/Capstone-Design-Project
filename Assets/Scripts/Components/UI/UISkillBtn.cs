using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillBtn : MonoBehaviour
{
    private bool isDelay = false;
    private int coolTime=5;
    public Image imgCoolTime;
    public TMP_Text txtCoolTime;
    // Start is called before the first frame update
    
    public void Init()                      //초기화
    {
        //cooltime 이미지의 fillAmount = 1
        this.imgCoolTime.fillAmount = 1;
        //txtCoolTime 비활성화
        this.txtCoolTime.gameObject.SetActive(false);
    }

    public void skill_cool()
    {
        if (this.isDelay) return;

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
        
        Debug.Log("스킬 사용가능");
        //txtCoolTime 비활성화
        this.txtCoolTime.gameObject.SetActive(false);
        this.imgCoolTime.fillAmount = 1;
    }
}
