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
    // [ReadOnly] public bool camPlayerLock = true;
    public Player player;
    public CameraController cam;
    public float camSensitive = 0.25f;

    [AssetList(AutoPopulate = true, Path = "/Prefabs/Monsters/")]
    public List<Monsters.Monster> monsters;

    [Required] public TMP_Text camLockDescription;
    [Required] public TMP_Dropdown monsterSpawn_Dropdown;
    [Required] public MouseTooltip_TrainingGround tooltip;
    private Brush_TrainingGround brush;
    public BrushType brushType;
    Dictionary<string, Monsters.Monster> dic = new Dictionary<string, Monsters.Monster>();

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
        InputManager.Instance.AddPerformed(InputKey.LeftClick, LeftClickPerform);
        InputManager.Instance.AddPerformed(InputKey.F, CamAttach_OnOff);

        ChangeBrush(new IdleBrush_TrainingGround(this));
        player = FindObjectOfType<Player>();
        cam = Camera.main.GetComponent<CameraController>();
    }

    private void LateUpdate()
    {
        if (!cam.attached)
        {
            Vector2 delta = InputManager.Instance.GetWASD();
            if (delta.sqrMagnitude > 0)
            {
                Vector3 origin = cam.transform.position;
                origin.z += delta.y * camSensitive;
                origin.x += delta.x * camSensitive;
                cam.transform.position = origin;
            }
        }
    }

    public void LeftClickPerform(InputAction.CallbackContext context)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            brush.Execute();
        }
    }

    // Dropdown에서 item의 PointerClick이벤트에 추가해서 같은거 선택해도 인식하게 (onValueChanged 우회)
    public void MonsterSpawn_DropdownClick(BaseEventData baseEventData)
    {
        // Debug.Log(monsterSpawn_Dropdown.options[monsterSpawn_Dropdown.value].text); 
        ChangeBrush(new MonsterSpawnBrush_TrainingGround(this,
            dic[monsterSpawn_Dropdown.options[monsterSpawn_Dropdown.value].text]));
    }

    public void MonsterRemove_ButtonClick()
    {
        ChangeBrush(new MonsterRemoveBrush_TrainingGround(this));
    }

    public void MonsterRemoveAll_ButtonClick()
    {
        Monsters.Monster[] removes = FindObjectsByType<Monsters.Monster>(FindObjectsSortMode.None);
        for (int i = 0; i < removes.Length; i++)
        {
            removes[i].Die();
        }
        ChangeBrush(new IdleBrush_TrainingGround(this));
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

    public void CamAttach_OnOff(InputAction.CallbackContext context)
    {
        if (cam.attached)
        {
            cam.Detach();
            camLockDescription.text = "플레이어 시점: F (캠이동: Keyboard arrows)";
        }
        else
        {
            cam.AfterAttach();
            camLockDescription.text = "플레이어 시점 해제: F";
        }
    }
}

public enum BrushType
{
    Idle,
    MonsterSpawn,
    MonsterRemove
}