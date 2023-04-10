using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : MonoBehaviour
{
    public Slider hpBar;
    public Slider hpBackBar;

    private float maxHp = 100;
    private float curHp = 100;
    public UISkillBtn uiSkillBtn;
    public UIRoll uiRoll;
    public GameObject invenSet;
    float delay = 0.5f;

    void Start()
    {
        this.uiSkillBtn.Init();
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

        if (Input.GetKeyDown(KeyCode.A))
        {
            curHp -= 10;
            StartCoroutine(HandleHp());
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
    }

    IEnumerator HandleHp()
    {
        hpBar.value = (float)curHp / (float)maxHp;
        yield return new WaitForSeconds(0.5f);
        hpBackBar.value = (float)curHp / (float)maxHp;
    }
    
}
