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
    
    [AssetList(AutoPopulate = true, Path = "/Prefabs/Monsters/")]
    public List<Monster> monsters;

    public TMP_Dropdown monsterSpawn_Dropdown;

    private void Start()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            monsterSpawn_Dropdown.options.Add(new TMP_Dropdown.OptionData(monsters[i].gameObject.name));
        }

        // 값이 바뀔때만 호출됨...어떻게?
        // monsterSpawn_Dropdown.onValueChanged.AddListener(delegate { print(monsterSpawn_Dropdown); });
    }

    // Dropdown에서 item의 PointerClick이벤트에 추가해서 같은거 선택해도 인식하게 (onValueChanged 우회)
    public void MonsterSpawnClick(BaseEventData baseEventData)  
    {
        Debug.Log(monsterSpawn_Dropdown.options[monsterSpawn_Dropdown.value].text);
    }
}