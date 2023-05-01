using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Monsters.Skill
{
    public class SkillSet_General_Melee : SkillSet
    {
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
            HitBox hitbox = Instantiate(baseSkillEffect);
            hitbox.Particle_Play(heart);
        }

        void EndEngage()
        {
            monster.whileEngage = false;
        }

        public override void Terminate()
        {
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
                monster.whileEngage = false; // 나중에 삭제
                return;
            }

            monster.animator.SetTrigger("Skill00"); // 사용 가능한 스킬이 없으면 그냥 평타
        }
    }
}