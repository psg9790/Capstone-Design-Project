using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HudUI : MonoBehaviour
{
    public Slider hpBar;
    public Slider hpBackBar;
    public Image hpImage;
    public Image hpBackImage;
    
    private float maxHp = 100;
    private float curHp = 100;
    public UISkillBtn[] uiSkillBtn=new UISkillBtn[4];
    public PortionBtn portionBtn;
    public UIRoll uiRoll;
    public GameObject menuSet;
    public GameObject invenSet;
    float delay = 0.5f;
        
    void Start() 
    {
        for (int i = 0; i < 4; i++)
        {
            this.uiSkillBtn[i].Init();
        }
        
        Color color;
        color = hpBackImage.color;
        color.a= (float)0.9;
        hpBackImage.color = color;
        curHp = Player.Instance.heart.CUR_HP;
        
        this.uiRoll.Init();
        hpBar.value = (float)curHp / (float)maxHp;
        hpBackBar.value = (float)curHp / (float)maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!uiRoll.isDelay)
            {
                uiRoll.RollSkillCool();
            }
        }
        if (Input.GetButtonDown("Cancel") && (invenSet.activeSelf)==false)
        {
            if (menuSet.activeSelf)
            {
                Time.timeScale = 1;
                menuSet.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                menuSet.SetActive(true); 
            }
        }
        if (curHp != Player.Instance.heart.CUR_HP)             //curHp != Player.Instance.heart.CUR_HP;
        {
            curHp = Player.Instance.heart.CUR_HP;
            hpImage.fillAmount =  Mathf.Lerp(hpBar.value,(float)curHp / (float)maxHp,Time.deltaTime*30);
            hpBackImage.fillAmount =  Mathf.Lerp(hpBackBar.value,(float)curHp / (float)maxHp,Time.deltaTime*20);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (invenSet.activeSelf)
            {
                invenSet.SetActive(false);
            }
            else
            {
                invenSet.SetActive(true);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.uiSkillBtn[0].skill_cool();
        } else if (Input.GetKeyDown(KeyCode.W))
        {
            uiSkillBtn[1].skill_cool();
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            uiSkillBtn[2].skill_cool();
        } else if (Input.GetKeyDown(KeyCode.R))
        {
            uiSkillBtn[3].skill_cool();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            portionBtn.Portion_Use();
        }

        
    }

    IEnumerator HandleHp()
    {
        hpImage.fillAmount =  Mathf.Lerp(hpBar.value,(float)curHp / (float)maxHp,Time.deltaTime*20);
        yield return new WaitForSeconds(0.1f);
        hpBackImage.fillAmount =  Mathf.Lerp(hpBackBar.value,(float)curHp / (float)maxHp,Time.deltaTime*20);
    }
    
}
