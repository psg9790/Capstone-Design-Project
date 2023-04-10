// Player나 Monster에 별도 컴포넌트로 부착

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

[System.Serializable]
public class Heart : MonoBehaviour
{
    [FoldoutGroup("Events")] public UnityEvent OnDeath; // 죽을 때 실행될 이벤트, 각 객체에서 알맞는 죽는 처리를 listener에 추가할 것
    
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

    
    public void Increase_MAX_HP(float amount)
    {
    }

    public void Decrease_MAX_HP(float amount)
    {
    }

    public void Restore_CUR_HP(float amount)
    {
    }

    [FoldoutGroup("Functions")] [Button]
    public void RestoreAll_CUR_HP()
    {
        cur_hp = max_hp;
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

    public Damage Generate_Damage(Player player)
    {
        return null;
    }

    public Damage Generate_Damage(Monsters.Monster monster)
    {
        return null;
    }

    [FoldoutGroup("Functions")] [Button]
    public void Take_Damage(Damage _damage)
    {
        // 내부 처리
        cur_hp -= _damage.damage;
        
        Debug.Log("\""+gameObject.name + "\" took " + _damage.damage + " damage! : " + cur_hp + "/" + max_hp);

        if (cur_hp <= 0)
        {
            Debug.Log("dead");
            OnDeath.Invoke();
        }
    }

    [FoldoutGroup("Functions")] [Button]
    public void Take_Damage_DOT(Damage _damage, float _tik, float _time)
    {
        // 초 처리 
        
        if (cur_hp <= 0)
        {
            Debug.Log("dead");
            OnDeath.Invoke();
        }
    }
}