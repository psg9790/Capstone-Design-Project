using System;
using System.Collections;
using System.Collections.Generic;
using Monsters.FSM;
using UnityEngine;

namespace Monsters.Skill
{
    public class SkillSet_Boss1_Nightmare : SkillSet
    {
        private float clawAttack_cooltime = 9f;
        private float clawAttack_cooldown = 0f;
        public HitBox clawAttack;
        
        public HitBox baseAttack;

        private float hornAttack_cooltime = 15f;
        private float hornAttack_cooldown = 10f;
        
        void BaseAttack()
        {
            HitBox ba = Instantiate(baseAttack);
            ba.Particle_Play(heart);
        }

        void ClawAttack()
        {
            HitBox ba = Instantiate(clawAttack);
            ba.Particle_Play(heart);
        }

        void HornAttack()
        {
            
        }

        IEnumerator HornRush()
        {
            int mask =~ LayerMask.GetMask("Monster");
            yield return null;
        }

        IEnumerator clawcooldown()
        {
            while (clawAttack_cooldown >= 0)
            {
                yield return null;
                clawAttack_cooldown -= Time.deltaTime;
            }
        }

        IEnumerator horncooldown()
        {
            while (hornAttack_cooldown >= 0)
            {
                yield return null;
                hornAttack_cooldown -= Time.deltaTime;
            }
        }

        public override void DoPossibleEngage()
        {
            // if (monster.playerDist >= 9)
            // {
            //     if (hornAttack_cooldown <= 0)
            //     {
            //         hornAttack_cooldown = hornAttack_cooltime;
            //         monster.animator.SetTrigger("Skill02");
            //         StartCoroutine(horncooldown());
            //         return;
            //     }
            // }
            if (clawAttack_cooldown <= 0)
            {
                clawAttack_cooldown = clawAttack_cooltime;
                monster.animator.SetTrigger("Skill01");
                StartCoroutine(clawcooldown());
                return;
            }
            monster.animator.SetTrigger("Skill00"); // 사용 가능한 스킬이 없으면 그냥 평타
        }
    }
}