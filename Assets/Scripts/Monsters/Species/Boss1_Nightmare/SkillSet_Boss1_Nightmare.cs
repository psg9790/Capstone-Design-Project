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
        public Transform bulletPos;
        public HitBox baseAttack;

        private float clawAttack_cooltime = 9f;
        private float clawAttack_cooldown = 0f;
        public HitBox clawAttack;

        private float hornAttack_cooltime = 15f;
        private float hornAttack_cooldown = 0f;
        public HitBox hornAttack;

        private void Start()
        {
            heart.OnDeath.AddListener(GenerateHighItem);
        }

        void GenerateHighItem()
        {
            if(RecordLevelManager.Instance == null)
                ItemGenerator.Instance.GenerateItem(monster.transform, monster.heart.LEVEL + 2);
        }
        
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
            if(!monster.fsm.CheckCurState(EMonsterState.Dead))
                OnOverlapSphere();
        }


        void BaseAttack()
        {
            HitBox ba = Instantiate(baseAttack);
            ba.Particle_Play(heart);
            // ba.BulletParticle_Play(heart, bulletPos.position, bulletPos.forward);
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
                int mask = ~ (LayerMask.GetMask("Monster") | LayerMask.GetMask("Player"));
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
            SyncAnimationSpeed();

            if (monster.playerDist >= 8)
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

        // 스킬셋에 맞는 몬스터의 고유값을 공유하기 위해서 재정의를 사용했음
        private static List<float> atk_byLevel = new List<float>();
        private static List<float> hp_byLevel = new List<float>();
        private static List<float> def_byLevel = new List<float>();
        private static List<float> atkspeed_byLevel = new List<float>();
        private static List<float> movementspeed_byLevel = new List<float>();

        public override void SetMonsterStatByLevel(short level)
        {
            if (atk_byLevel.Count == 0) // 새로운 전역 레벨 변수 추가
            {
                float calcatk, calchp, calcdef, calcatkspeed, calcmovespeed;
                atk_byLevel.Add(calcatk = heart.ATK);
                hp_byLevel.Add(calchp = heart.MAX_HP);
                def_byLevel.Add(calcdef = heart.DEF);
                atkspeed_byLevel.Add(calcatkspeed = heart.ATK_SPEED);
                movementspeed_byLevel.Add(calcmovespeed = heart.MOVEMENT_SPEED);
                // for (int i = 0; i < GrowthLevelManager.Instance.maxLevel; i++)
                // {
                //     atk_byLevel.Add(calcatk *= statGrowthByLevelUp);
                //     hp_byLevel.Add(calchp *= statGrowthByLevelUp);
                //     def_byLevel.Add(calcdef *= statGrowthByLevelUp);
                //     atkspeed_byLevel.Add(calcatkspeed += (statGrowthByLevelUp * 0.05f));
                //     movementspeed_byLevel.Add(calcmovespeed += (statGrowthByLevelUp * 0.05f));
                // }
            }

            if (atk_byLevel.Count <= level)
            {
                for (int i = atk_byLevel.Count - 1; i < level; i++)
                {
                    atk_byLevel.Add(atk_byLevel[i] *= statGrowthByLevelUp);
                    hp_byLevel.Add(hp_byLevel[i] *= statGrowthByLevelUp);
                    def_byLevel.Add(def_byLevel[i] *= statGrowthByLevelUp);
                    atkspeed_byLevel.Add(atkspeed_byLevel[i] += (statGrowthByLevelUp * 0.05f));
                    movementspeed_byLevel.Add(movementspeed_byLevel[i] += (statGrowthByLevelUp * 0.05f));
                }
            }

            heart.SetStat(atk_byLevel[level], hp_byLevel[level], def_byLevel[level],
                atkspeed_byLevel[level], movementspeed_byLevel[level]);
        }

        private HPbar_custom boss_hpbar;

        private void OnOverlapSphere() // 보스 hp바
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, 20f, 1 << LayerMask.NameToLayer("Player"));
            if (boss_hpbar == null)
            {
                boss_hpbar = GameObject.Find("Canvas").transform.Find("boss_hpbar").gameObject.GetComponent<HPbar_custom>();
            }
            if (cols.Length > 0)
            {
                if(!boss_hpbar.isActiveAndEnabled)
                {
                    boss_hpbar.bossNameText.text = "고대드래곤";
                    boss_hpbar.Activate(heart);
                    boss_hpbar.gameObject.SetActive(true);
                }
            }
            else
            {
                boss_hpbar.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if(boss_hpbar != null)
                if(boss_hpbar.heart == heart)
                    boss_hpbar.gameObject.SetActive(false);
        }
    }
}