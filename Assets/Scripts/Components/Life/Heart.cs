using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Heart : MonoBehaviour
{
    [FoldoutGroup("Attributes")]
    [InfoBox("이 변수들을 플레이 중 직접 수정하면 다른 오브젝트 작동 시 (장비 장착/해제 등) 오류가 발생할 수도 있습니다.")]
    [SerializeField] private float max_hp;
    [FoldoutGroup("Attributes")][SerializeField] private float cur_hp;
    [FoldoutGroup("Attributes")][SerializeField] private float atk;
    [FoldoutGroup("Attributes")][SerializeField] private float def;
    [FoldoutGroup("Attributes")][SerializeField] private float movement_speed;
    [FoldoutGroup("Attributes")][SerializeField] private float atk_speed;
    [FoldoutGroup("Attributes")][SerializeField] private float skill_cooldown;
    
    public float MAX_HP => max_hp;
    public float CUR_HP => cur_hp;
    public float ATK => atk;
    public float DEF => def;
    public float MOVEMENT_SPEED => movement_speed;
    public float ATK_SPEED => atk_speed;
    public float SKILL_COOLDOWN => skill_cooldown;

    [HideInInspector] public UnityEvent deathevent;    // 이벤트 등록 필요
    
    public void Increase_MAX_HP(float amount)
    {
    }

    public void Decrease_MAX_HP(float amount)
    {
    }

    public void Restore_CUR_HP(float amount)
    {
    }

    public void Increase_DEF(float amount)
    {
    }

    public void Decrease_DEF(float amount)
    {
    }

    public void Increase_ATK_SPEED(float amount)
    {
    }

    public void Decrease_ATK_SPEED(float amount)
    {
    }

    public void Increase_MOVEMENT_SPEED(float amount)
    {
    }

    public void Decrease_MOVEMENT_SPEED(float amount)
    {
    }

    public void Increase_SKILL_COOLDOWN(float amount)
    {
    }

    public void Decrease_SKILL_COOLDOWN(float amount)
    {
    }

    public Damage GenerateDamage(Player player)
    {
        return null;
    }

    public Damage GenerateDamage(Monster monster)
    {
        return null;
    }

    public void TakeDamage_Impulse(Damage _damage)
    {
    }

    public void TakeDamage_DamageOverTime(Damage _damage, float _tik, float _time)
    {
    }
}