using System;
using System.Collections;
using System.Collections.Generic;
using Monsters.FSM;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Monsters.Skill
{
    public class SkillSet_Boss1_Nightmare : SkillSet
    {
        public HitBox baseAttack;

        private float clawAttack_cooltime = 9f;
        private float clawAttack_cooldown = 0f;
        public HitBox clawAttack;

        private float hornAttack_cooltime = 15f;
        private float hornAttack_cooldown = 0f;
        public HitBox hornAttack;

        private void Update()
        {
            if (hornAttack_cooldown <= 0)
            {
                if (monster.fsm.CheckCurState(EMonsterState.ChasePlayer))
                {
                    if (monster.playerDist >= 9f)
                    {
                        monster.fsm.ChangeState(EMonsterState.Engage);
                    }
                }
            }
        }


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
            HitBox ha = Instantiate(hornAttack);
            ha.Particle_Play(heart);
            StartCoroutine(HornRush(ha));
        }

        IEnumerator HornRush(HitBox hitBox)
        {
            monster.nav.enabled = false;
            monster.rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            // monster.rigid.velocity = monster.transform.forward * 20;
            while (hitBox != null)
            {
                int mask = ~ LayerMask.GetMask("Monster");
                if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward * 2f, 1f, mask))
                {
                    transform.position = transform.position
                                         + Vector3.Lerp(transform.forward * 35, Vector3.zero,
                                             (hitBox.elapsed / hitBox.duration)) * Time.deltaTime;
                }

                hitBox.transform.position = monster.transform.position;
                yield return null;
            }

            monster.nav.enabled = true;
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
            if (monster.playerDist >= 6)
            {
                if (hornAttack_cooldown <= 0)
                {
                    hornAttack_cooldown = hornAttack_cooltime;
                    monster.animator.SetTrigger("Skill02");
                    StartCoroutine(horncooldown());
                    return;
                }
            }
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