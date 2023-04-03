using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillArchive : MonoBehaviour
{
    public Collider baseAttack_col;
    public Collider skill01_col;
    public Collider skill02_col;

    public void BaseAttackCollider_ON()
    {
        baseAttack_col.enabled = true;
        baseAttack_col.GetComponent<MonsterSkillHitBox>().ClearHash(); // 해시 초기화
    }

    public void BaseAttackCollider_OFF()
    {
        baseAttack_col.enabled = false;
    }

    public void Skill01Collider_ON()
    {
        
    }

    public void Skill01Collider_OFF()
    {
        
    }

    public void Skill02Collider_ON()
    {
        
    }

    public void Skill02Collider_OFF()
    {
        
    }
}
