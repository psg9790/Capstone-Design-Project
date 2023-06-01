using System;
using System.Collections;
using System.Collections.Generic;
using Monsters.FSM;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Monsters.Skill
{
    public class SkillSet_Boss3_Necromancer : SkillSet
    {
        public Transform rightTransform;

        private void Start()
        {
            if (siblingTransform == null)
            {
                siblingTransform = new GameObject("sibling").transform;
            }
            heart.OnDeath.AddListener(GenerateHighItem);
            heart.OnDeath.AddListener(RemoveSiblings);
        }

        void GenerateHighItem()
        {
            if (RecordLevelManager.Instance == null)
                ItemGenerator.Instance.GenerateItem(monster.transform, monster.heart.LEVEL + 2);
        }

        public HitBox baseAttackVFX;

        void BaseAttack()
        {
            if (baseAttackVFX != null)
            {
                HitBox baseAttack = Instantiate(baseAttackVFX);
                baseAttack.BulletParticle_Play(heart, transform.position + Vector3.up, transform.forward);
            }
        }

        public HitBox spreadAttackVFX;

        void SpreadAttack()
        {
            if (spreadAttackVFX != null)
            {
                HitBox spread = Instantiate(spreadAttackVFX);
                Vector3 dir = rightTransform.position - transform.position;
                dir.y = 0;
                dir = dir.normalized;
                spread.BulletParticle_Play(heart, transform.position + Vector3.up, dir);
            }
        }

        public HitBox raserVFX;

        void RaserAttack()
        {
            if (raserVFX != null)
            {
                HitBox raser = Instantiate(raserVFX);
                raser.transform.SetParent(this.transform);
                raser.Particle_Play(heart);
            }
        }

        public HitBox throwVFX;

        void ThrowAttack()
        {
            StartCoroutine(ThrowAttackIE());
        }

        IEnumerator ThrowAttackIE()
        {
            if (throwVFX != null)
            {
                HitBox throwAttack = Instantiate(throwVFX);
                throwAttack.BulletParticle_Play(heart, rightTransform.position, Vector3.up);
            }

            yield return new WaitForSeconds(1.5f);
            if (throwVFX != null)
            {
                HitBox throwAttack = Instantiate(throwVFX);
                throwAttack.BulletParticle_Play(heart, Player.Instance.transform.position + Vector3.up * 20f,
                    -Vector3.up);
            }
        }

        private Transform siblingTransform;
        public HitBox spawnVFX;

        void SpawnMonsters()
        {
            

            HitBox spawn = Instantiate(spawnVFX);
            spawn.Particle_Play(heart);
            StartCoroutine(SpawnMonstersIE());
        }

        IEnumerator SpawnMonstersIE()
        {
            SpawnMonster();
            yield return new WaitForSeconds(0.25f);
            SpawnMonster();
            yield return new WaitForSeconds(0.25f);
            SpawnMonster();
            yield return new WaitForSeconds(0.25f);
            SpawnMonster();
            yield return new WaitForSeconds(0.25f);
        }

        private GameObject[] general_monsters = new GameObject[0];

        void SpawnMonster()
        {
            Vector3 pos
                = transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            if (general_monsters.Length <= 0)
                general_monsters = Resources.LoadAll<GameObject>("Monsters/General/");
            int rndGeneralMonster = UnityEngine.Random.Range(0, general_monsters.Length);
            Monsters.Monster newMonster =
                Instantiate(general_monsters[rndGeneralMonster]).GetComponent<Monsters.Monster>();
            newMonster.transform.SetParent(siblingTransform);
            newMonster.transform.rotation = Quaternion.LookRotation(transform.forward);
            newMonster.Init(pos, 8f);
            newMonster.heart.SetMonsterStatByLevel((short)heart.LEVEL);
            newMonster.fov.viewRadius = monster.fov.viewRadius;
        }

        void RemoveSiblings()
        {
            if (siblingTransform != null)
            {
                Heart[] hearts = siblingTransform.GetComponentsInChildren<Heart>();
                for (int i = 0; i < hearts.Length; i++)
                {
                    hearts[i].ForceDead();
                }
                Destroy(siblingTransform.gameObject);
            }
        }

        private float spreadCooldown = 0;
        private float spreadCooltime = 8f;
        private float throwCooldown = 0f;
        private float throwCooltime = 10f;
        private float raserCooldown = 0f;
        private float raserCooltime = 30f;
        private float spawnCooldown = 0f;
        private float spawnCooltime = 50f;

        public override void DoPossibleEngage()
        {
            SyncAnimationSpeed();
            if (spawnCooldown <= 0)
            {
                spawnCooldown = spawnCooltime;
                monster.animator.SetTrigger("SpawnMonsters");
                return;
            }

            if (spreadCooldown <= 0)
            {
                spreadCooldown = spreadCooltime;
                monster.animator.SetTrigger("SpreadAttack");
                return;
            }

            if (throwCooldown <= 0)
            {
                throwCooldown = throwCooltime;
                monster.animator.SetTrigger("ThrowAttack");
                return;
            }

            if (raserCooldown <= 0)
            {
                raserCooldown = raserCooltime;
                monster.animator.SetTrigger("RaserAttack");
                return;
            }

            monster.animator.SetTrigger("BaseAttack");
        }

        private void Update()
        {
            raserCooldown -= Time.deltaTime;
            throwCooldown -= Time.deltaTime;
            spreadCooldown -= Time.deltaTime;
            spawnCooldown -= Time.deltaTime;
            if(!monster.fsm.CheckCurState(EMonsterState.Dead))
                OnOverlapSphere();
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
                boss_hpbar = GameObject.Find("Canvas").transform.Find("boss_hpbar").gameObject
                    .GetComponent<HPbar_custom>();
            }

            if (cols.Length > 0)
            {
                if (!boss_hpbar.isActiveAndEnabled)
                {
                    boss_hpbar.bossNameText.text = "네크로맨서";
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
            if (boss_hpbar != null)
                if (boss_hpbar.heart == heart)
                    boss_hpbar.gameObject.SetActive(false);
        }

        #region lerp

        private Coroutine lerpBaseAttackCo;
        private IEnumerator lerpIE(float value)
        {
            while (true)
            {
                Vector3 dir = Player.Instance.transform.position - transform.position;
                dir.y = 0;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), value);
                yield return null;
            }
        }
        public void LerpBeforeBaseAttack(float value)
        {
            lerpBaseAttackCo = StartCoroutine(lerpIE(value));
        }

        public void LerpTerminate()
        {
            if (lerpBaseAttackCo != null)
            {
                StopCoroutine(lerpBaseAttackCo);
            }
        }
        #endregion

    }
}