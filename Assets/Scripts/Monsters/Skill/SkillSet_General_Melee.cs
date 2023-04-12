using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.Skill
{
    public class SkillSet_General_Melee : SkillSet
    {
        public HitBox baseHitBox;

        void BaseHitOn()
        {
            baseHitBox.COLLIDER_ON(new Damage(heart.ATK, CC_type.None));
        }

        void BaseHitOff()
        {
            baseHitBox.COLLIDER_OFF();
        }
        
        public override void Terminate()
        {
            baseHitBox.COLLIDER_OFF();
        }
    }
}