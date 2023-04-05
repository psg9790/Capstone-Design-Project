using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillArchive : MonoBehaviour
{
    public HitBox attack01;
    public HitBox skill01;
    public HitBox skill02;

    public void BaseAttackCollider_ON()
    {
        attack01.COLLIDER_ON(new Damage(10, CC_type.None));
        // attack01.enabled = true;
        // attack01.GetComponent<HitBox>().ClearHash(); // 해시 초기화
    }

    public void BaseAttackCollider_OFF()
    {
        attack01.COLLIDER_OFF();
        // attack01.enabled = false;
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
