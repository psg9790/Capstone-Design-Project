using System.Collections;
using System.Collections.Generic;
using Monsters.FSM;
using UnityEngine;

namespace Monsters.Skill
{
    public class SkillSet_Rush_Spider : SkillSet
    {
        private float skill01_coolDown = 0f;
        private float skill01_coolTime = 7f;
        private float skill01_range = 7f;

        public HitBox skill01_Effect;
        public HitBox baseSkill_Effect;
        
        void Update()
        {
            if (skill01_coolDown > 0)
            {
                skill01_coolDown -= Time.deltaTime;
            }
            else
            {
                if (monster.fsm.CheckCurState(EMonsterState.ChasePlayer))
                {
                    monster.fsm.ChangeState(EMonsterState.Engage);
                }
            }
        }

        void BaseAttack()
        {
            HitBox hitBox =  Instantiate(baseSkill_Effect);
            hitBox.Particle_Play(heart);
        }
        
        void Skill01_Rush()
        {
            monster.rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            monster.rigid.velocity = monster.transform.forward * 20;
            HitBox hitBox =  Instantiate(skill01_Effect);
            hitBox.Particle_Play(heart);
            StartCoroutine(Skill01_effectFollow(hitBox));
        }

        IEnumerator Skill01_effectFollow(HitBox hitBox)
        {
            while (monster.whileEngage)
            {
                yield return null;
                if (hitBox == null)
                    break;
                monster.rigid.velocity = Vector3.Lerp(monster.transform.forward * 20, Vector3.zero,
                    (hitBox.elapsed / hitBox.duration));
                // UnityEngine.Debug.Log((hitBox.duration - hitBox.elapsed) / hitBox.duration);
                // UnityEngine.Debug.Log(hitBox.duration.ToString() + hitBox.elapsed.ToString());

                hitBox.transform.position = monster.transform.position;
            }
        }

        public override void DoPossibleEngage()
        {
            if (skill01_coolDown <= 0)
            {
                monster.rigid.constraints = RigidbodyConstraints.FreezeAll;
                monster.animator.SetTrigger("Skill01");
                skill01_coolDown = skill01_coolTime;
                return;
            }
            monster.animator.SetTrigger("Skill00"); // 사용 가능한 스킬이 없으면 그냥 평타
        }
    }
}