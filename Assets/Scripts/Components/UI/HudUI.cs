using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudUI : MonoBehaviour
{
    public UISkillBtn uiSkillBtn;
    public UIRoll uiRoll;
    void Start()
    {
        this.uiSkillBtn.Init();
        this.uiRoll.Init();
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
    }
    
}
