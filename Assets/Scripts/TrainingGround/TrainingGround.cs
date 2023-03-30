using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TrainingGround : MonoBehaviour
{
    [ReadOnly] public bool camPlayerLock = true;
    public Player player;
    
    [AssetList(AutoPopulate = true, Path = "/Prefabs/Monsters/")]
    public List<Monster> monsters;

    [Required] public TMP_Dropdown monsterSpawn_Dropdown;
    [Required] public MouseTooltip_TrainingGround tooltip;
    private Brush_TrainingGround brush;
    public BrushType brushType;
    Dictionary<string, Monster> dic = new Dictionary<string, Monster>();

    private void Start()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            monsterSpawn_Dropdown.options.Add(new TMP_Dropdown.OptionData(monsters[i].gameObject.name));
        }
        for (int i = 0; i < monsters.Count; i++)
        {
            dic.Add(monsters[i].name, monsters[i]);
        }

        InputManager.Instance.AddPerformed(InputKey.RightClick, CancleBrush);
        ChangeBrush(new IdleBrush_TrainingGround(this));
        player = FindObjectOfType<Player>();
    }

    // Dropdown에서 item의 PointerClick이벤트에 추가해서 같은거 선택해도 인식하게 (onValueChanged 우회)
    public void MonsterSpawnClick(BaseEventData baseEventData)  
    {
        // Debug.Log(monsterSpawn_Dropdown.options[monsterSpawn_Dropdown.value].text); 
        ChangeBrush(new MonsterSpawnBrush_TrainingGround(this, dic[monsterSpawn_Dropdown.options[monsterSpawn_Dropdown.value].text]));
    }

    // brush 바꾸는 메서드
    public void ChangeBrush(Brush_TrainingGround next)
    {
        if (!ReferenceEquals(brush, null))
        {
            brush.Exit();
        }
        brush = next;
        brush.Enter();
    }

    public void CancleBrush(InputAction.CallbackContext context)
    {
        if (brushType != BrushType.Idle)
        {
            ChangeBrush(new IdleBrush_TrainingGround(this));
            // 플레이어 이동을 막을 방법?
            // player.nav.ResetPath();
        }
    }
}

public enum BrushType
{
    Idle,
    MonsterSpawn
}