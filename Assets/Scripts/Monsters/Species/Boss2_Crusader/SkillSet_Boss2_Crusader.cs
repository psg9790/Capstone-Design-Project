using System;
using System.Collections;
using System.Collections.Generic;
using Monsters.FSM;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Monsters.Skill
{
    public class SkillSet_Boss2_Crusader : SkillSet
    {
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
            if(!monster.fsm.CheckCurState(EMonsterState.Dead))
                OnOverlapSphere();
        }

        private Coroutine lerpBaseAttackCo;
        private IEnumerator lerpIE()
        {
            while (true)
            {
                Vector3 dir = Player.Instance.transform.position - transform.position;
                dir.y = 0;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 0.025f);
                yield return null;
            }
        }
        public void LerpBeforeBaseAttack()
        {
            lerpBaseAttackCo = StartCoroutine(lerpIE());
        }

        public void LerpTerminate()
        {
            if (lerpBaseAttackCo != null)
            {
                StopCoroutine(lerpBaseAttackCo);
            }
        }
        public HitBox baseAttack_hitbox;
        void BaseAttack()
        {
            LerpTerminate();
            if (baseAttack_hitbox != null)
            {
                HitBox atk = Instantiate(baseAttack_hitbox);
                atk.Particle_Play(heart);
            }
        }

        public HitBox shieldAttack_hitbox;
        void ShieldAttack()
        {
            LerpTerminate();
            if (shieldAttack_hitbox != null)
            {
                HitBox atk = Instantiate(shieldAttack_hitbox);
                atk.Particle_Play(heart);
            }
        }

        private bool canAttack2 = true;
        private Coroutine Attack2CooldownCo;
        private float attack2Cooltime = 8f;
        public HitBox attack2_hitbox;
        void Attack2()
        {
            canAttack2 = false;
            Attack2CooldownCo = StartCoroutine(Attack2CooldownIE());
            if (attack2_hitbox != null)
            {
                HitBox atk = Instantiate(attack2_hitbox);
                atk.Particle_Play(heart);
            }
        }

        private IEnumerator Attack2CooldownIE()
        {
            float elapsed = 0;
            while (elapsed < attack2Cooltime)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            canAttack2 = true;
        }
        
        private bool canAttack3 = true;
        private Coroutine Attack3CooldownCo;
        private float attack3Cooltime = 12f;
        public HitBox attack3_hitbox;
        void Attack3()
        {
            canAttack3 = false;
            Attack3CooldownCo = StartCoroutine(Attack3CooldownIE());
            if (attack3_hitbox != null)
            {
                HitBox atk = Instantiate(attack3_hitbox);
                atk.Particle_Play(heart);
            }
        }

        private IEnumerator Attack3CooldownIE()
        {
            float elapsed = 0;
            while (elapsed < attack3Cooltime)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            canAttack3 = true;
        }
        
        
        public override void DoPossibleEngage()
        {
            SyncAnimationSpeed();
            if (canAttack3)
            {
                monster.animator.SetTrigger("Attack3");
                return;
            }
            
            if (canAttack2)
            {
                monster.animator.SetTrigger("Attack2");
                return;
            }
            
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                monster.animator.SetTrigger("BaseAttack");
            }
            else
            {
                monster.animator.SetTrigger("ShieldAttack");
            }

            LerpBeforeBaseAttack();
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
                    boss_hpbar.bossNameText.text = "암흑기사";
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