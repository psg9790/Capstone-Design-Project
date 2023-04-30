using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Monsters.Skill
{
    public class SkillSet_General_Melee : SkillSet
    {
        // [FoldoutGroup("HitBoxes")] [Required] public HitBox baseHitBox;
        public HitBox baseSkillEffect;
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
            // baseSkillEffect.SetDamage(heart.Generate_Damage(1, CC_type.None, 0));
            HitBox hitbox = Instantiate(baseSkillEffect);
            hitbox.Particle_Play(heart);
            // baseHitBox.COLLIDER_ON(heart.Generate_Damage(1, CC_type.None, 0));
        }

        // void BaseHitOff() // collider enabled = false;
        // {
        //     baseSkillEffect.Particle_Stop();
        //     // baseHitBox.COLLIDER_OFF();
        // }

        void EndEngage()
        {
            monster.whileEngage = false;
        }

        public override void Terminate()
        {
            // BaseHitOff();
            // baseSkillEffect.Particle_Stop();
            // 스킬 취소 없음? 관대한 캔슬
            EndEngage();
        }

        public override void DoPossibleEngage()
        {
            if (skill01_coolDown <= 0) // 스킬1 시전 가능하면 시전
            {
                skill01_coolDown = SKILL01;
                // 애니메이션 재생
                // Debug.Log("skill01");
                monster.whileEngage = false;
                return;
            }

            monster.animator.SetTrigger("Skill00"); // 사용 가능한 스킬이 없으면 그냥 평타
        }
    }
}