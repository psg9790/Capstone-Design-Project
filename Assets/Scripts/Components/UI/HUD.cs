using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType
    {
        Hp,
        RollCool,
        SkillCool,
        SkillCool2
    }
    
    public InfoType type;
        
    Text text;
    Slider MySlider;

    private void Awake()
    {
        text = GetComponent<Text>();
        MySlider = GetComponent<Slider>();
    }

    
    
    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Hp:
                float CurrHp = GameManager.Instance.Hp;
                float MaxHp = GameManager.Instance.MaxHp;
                MySlider.value = CurrHp / MaxHp;
                break;
            case InfoType.RollCool:

                break;
            case InfoType.SkillCool:

                break;
            case InfoType.SkillCool2:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
