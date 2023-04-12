using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.Skill
{
    public class SkillSet_General_Melee : SkillSet
    {
        public HitBox baseHitBox;

        void BaseHitOn() // 기본 검 공격 collider enabled = true;
        {
            baseHitBox.COLLIDER_ON(new Damage(heart.ATK, CC_type.None));
        }

        void BaseHitOff() // collider enabled = false;
        {
            baseHitBox.COLLIDER_OFF();
        }
        
        public override void Terminate()
        {
            baseHitBox.COLLIDER_OFF();
        }
    }
}