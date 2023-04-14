using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Monsters.Skill
{
    public class SkillSet_General_Melee : SkillSet
    {
        [FoldoutGroup("HitBoxes")] [Required] public HitBox baseHitBox;
        [ReadOnly] public float skill01_coolDown;
        private float SKILL01 = 5f;


        void Update()
        {
            if (skill01_coolDown > 0)
            {
                skill01_coolDown -= Time.deltaTime;
            }
        }

        void BaseHitOn() // 기본 검 공격 collider enabled = true;
        {
            baseHitBox.COLLIDER_ON(new Damage(monster.heart.ATK, CC_type.None));
        }

        void BaseHitOff() // collider enabled = false;
        {
            baseHitBox.COLLIDER_OFF();
        }

        void EndEngage()
        {
            monster.engage = false;
        }
        
        public override void Terminate()
        {
            baseHitBox.COLLIDER_OFF();
        }

        public override void DoPossibleEngage()
        {
            if (skill01_coolDown <= 0)
            {
                skill01_coolDown = SKILL01;
                // 애니메이션 재생
                Debug.Log("skill01");
                monster.engage = false;
                return;
            }

            monster.animator.SetTrigger("Skill00"); // 사용 가능한 스킬이 없으면 그냥 평타
        }
    }
}