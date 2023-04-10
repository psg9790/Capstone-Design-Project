// 몬스터 최상위 클래스
// 앞으로 구현할 세부 몬스터들은 이 클래스를 상속받아서 구현하게 될 것

using System.Collections;
using System.Collections.Generic;
using System.Net;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Monsters
{

    public class Monster : MonoBehaviour
    {
        public EMonsterType monsterType;
        // 체력, 스탯 관련
        public Heart heart;

        // components
        [HideInInspector] public NavMeshAgent nav; // 컴포넌트 미리 추가 필요
        [HideInInspector] public Animator animator; // 컴포넌트 미리 추가 필요
        [HideInInspector] public MonsterFOV fov; // 시야, 컴포넌트 미리 추가 필요
        [HideInInspector] public MonsterSkillArchive skills; // 스킬 판정 collider를 넣어둘 컴포넌트 (미완)

        // behavior
        [HideInInspector]
        public MonsterStateMachine fsm; // 상태의 변경을 관리할 장치, 상태 변경 시 이전 상태의 Exit() 실행 후 새로운 상태의 Enter()를 실행시켜 줌

        [ReadOnly] public EMonsterState state; // 몬스터의 현재 상태를 표시할 열거형 변수, 현재 상태가 무엇인지 검사하는데 쓰임

        // spawner & player
        [HideInInspector] public Player player; // 플레이어가 시야에 있을 시 플레이어 정보를 넣어줄 변수
        [BoxGroup("Spawn")] [ReadOnly] public Vector3 spawnPoint; // 몬스터가 스폰된 위치
        [BoxGroup("Spawn")] [ReadOnly] public float patrolRadius; // 몬스터가 스폰 위치로부터 순찰할 반지름 거리

        // battle
        [BoxGroup("Battle")] public float attackRange = 1.75f; // 몬스터의 공격 사정거리
        [BoxGroup("Battle")] [ReadOnly] public bool whileAttack = false; // 공격할 때 키고, 끝나면 끌 플래그

        // fov
        [HideInInspector] public float BASE_FOV_RADIUS; // 몬스터의 기본 시야 범위를 저장
        [HideInInspector] public float BASE_FOV_ANGLE; // 몬스터의 기본 시야각을 저장

        [BoxGroup("FOV")]
        public float extendFovRadius_multi = 2f; // 흥분상태의 확장 시야 범위를 결정할 변수. 기본 시야 범위에 곱해진다. 인스펙터에서 수정할 것.

        [BoxGroup("FOV")] [Range(0, 360)]
        public float extendFovAngle = 360f; // 흥분상태의 확장 시야각을 결정할 변수. 이 각으로 덮어씌워진다. 인스펙터에서 수정.

        [BoxGroup("FOV")] public float extendFovTime = 4f; // 흥분 상태에서 기본상태로 전환될 시간
        [BoxGroup("FOV")] [ReadOnly] public bool extendedSight = false; // 흥분 상태
        [BoxGroup("FOV")] [ReadOnly] public bool playerInSight = false; // 플레이어 정보 저장에 있어서 null체크를 줄이기 위해 bool값으로 따로 관리
        [BoxGroup("FOV")] [ReadOnly] public float playerDist = -1f; // 플레이어가 시야에 있으면 거리를 갱신해줌

        // infos
        [HideInInspector] public float idleElapsedTime = 0f;
        [HideInInspector] public float idleEndTime = 4f;
        [HideInInspector] public float catchPatrolRaceCondition = 0;


        public void Spawn(Vector3 pos, float range)
        {
            this.transform.position = pos;
            spawnPoint = pos;
            patrolRadius = range;
        }

        void Awake()
        {
            OnAwake();
        }

        void Update()
        {
            OnUpdate();
        }

        protected virtual void OnAwake()
        {
            if (TryGetComponent<Heart>(out Heart _heart))
            {
                heart = _heart;
            }
            else
            {
                Debug.LogError(this.gameObject.name + " 몬스터에 \"Heart\" 컴포넌트가 없습니다.");
            }

            if (TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
            {
                nav = agent;
                nav.updateRotation = false;
            }
            else
            {
                Debug.LogError(this.gameObject.name + " 몬스터에 \"NavMeshAgent\" 컴포넌트가 없습니다.");
            }

            if (TryGetComponent<Animator>(out Animator anim))
            {
                animator = anim;
            }
            else
            {
                Debug.LogError(this.gameObject.name + " 몬스터에 \"Animator\" 컴포넌트가 없습니다.");
            }

            if (TryGetComponent<MonsterFOV>(out MonsterFOV _fov))
            {
                fov = _fov;
                BASE_FOV_RADIUS = fov.viewRadius;
                BASE_FOV_ANGLE = fov.viewAngle;
            }
            else
            {
                UnityEngine.Debug.LogError(this.gameObject.name + " 몬스터에 \"MonsterFOV\" 컴포넌트가 없습니다.");
            }

            if (TryGetComponent<MonsterSkillArchive>(out MonsterSkillArchive archive))
            {
                skills = archive;
            }
            else
            {
                Debug.LogWarning(this.gameObject.name + " 몬스터에 \"MonsterSkillArchive\" 컴포넌트가 없습니다.");
            }

            fsm = new MonsterStateMachine(this);
            fsm.ChangeState(EMonsterState.Idle);
        }

        protected virtual void OnUpdate()
        {
            // 현재 state행동 update
            fsm.Execute();

            // nav용 rotation
            NavRotation();

            // 시야에 플레이어가 있는지 갱신
            fov.FindVisiblePlayer();

            // 기본 행동
            switch (state)
            {
                case EMonsterState.Idle: // 대기 상태
                    Idle_Coditions();
                    break;

                case EMonsterState.Patrol: // 순찰 상태
                    Patrol_Conditions();
                    break;

                case EMonsterState.ChasePlayer: // 추적 상태
                    ChasePlayer_Conditions();
                    break;

                case EMonsterState.BaseAttack: // 기본 공격 수행 중
                    BaseAttack_Conditions();
                    break;

                case EMonsterState.Runaway:
                    Runaway_Conditions();
                    break;

                case EMonsterState.Dead:
                    Dead_Conditions();
                    break;

                case EMonsterState.Stiff:
                    Stiff_Conditions();
                    break;
            }
        }

        protected virtual void Idle_Coditions()
        {
            if (playerInSight) // 플레이어가 시야에 들어오면
            {
                // https://forum.unity.com/threads/getting-the-distance-in-nav-mesh.315846/
                if (playerDist > attackRange) // 타깃이 공격 사정거리보다 멀면
                {
                    fsm.ChangeState(EMonsterState.ChasePlayer);
                }
                else // 타깃이 공격 사정거리 안이면
                {
                    // 공격 쿨타임 추가?
                    fsm.ChangeState(EMonsterState.BaseAttack);
                }
            }
        }

        protected virtual void Patrol_Conditions()
        {
            if (playerInSight) // 플레이어 발견시
            {
                fsm.ChangeState(EMonsterState.ChasePlayer);
                return;
            }

            if (catchPatrolRaceCondition > 1.1f) // 너무 오래 일정속도 이하로 있으면
            {
                Debug.Log("stop!!!");
                catchPatrolRaceCondition = 0;
                fsm.ChangeState(EMonsterState.Idle);
                return;
            }

            if (nav.velocity.sqrMagnitude > 3.5f) // 일정 속도 이상으로 움직이고 있으면 0으로 초기화
                catchPatrolRaceCondition = 0;
            else
                catchPatrolRaceCondition += Time.deltaTime; // 정상 속도로 움직일 시 계속 0으로 초기화됨.
        }

        protected virtual void ChasePlayer_Conditions()
        {
            if (playerInSight) // 네비게이션이 경로 탐색을 완료했고
            {
                if (playerDist < attackRange) // 플레이어가 공격 사정거리 안에 들어왔을 때
                {
                    fsm.ChangeState(EMonsterState.BaseAttack);
                }
            }
            else // 플레이어를 시야에서 놓쳤을 시
            {
                fsm.ChangeState(EMonsterState.Idle);
            }
        }

        protected virtual void BaseAttack_Conditions()
        {
            if (!whileAttack) // "공격중" 플래그가 꺼지면 (애니메이션 마지막에 이벤트로 끔)
                fsm.ChangeState(EMonsterState.Idle);
        }

        protected virtual void Runaway_Conditions()
        {

        }

        protected virtual void Dead_Conditions()
        {

        }

        protected virtual void Stiff_Conditions()
        {

        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////
        /// </summary>

        protected void NavRotation()
        {
            // https://srdeveloper.tistory.com/115
            if (!nav.hasPath)
                return;

            Vector2 forward = new Vector2(transform.position.z, transform.position.x);
            Vector2 steeringTarget = new Vector2(nav.steeringTarget.z, nav.steeringTarget.x);

            // 방향을 구한 뒤, 역함수로 각을 구한다.
            Vector2 dir = steeringTarget - forward;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // 방향 적용
            transform.eulerAngles = Vector3.up * angle;
        }

        /// <summary>
        /// 시야 증가 코루틴
        /// </summary>
        private Coroutine extendSightCo; // 이 변수에 시야증가 코루틴을 담음, 새로운 시야증가 메서드 호출 시 이전 코루틴을 중지하고 새로운 코루틴을 실행

        private IEnumerator SightCo() // 위 extendSightCo 변수에 할당할 코루틴 
        {
            fov.viewRadius = BASE_FOV_RADIUS * extendFovRadius_multi; // 시야 증가
            fov.viewAngle = extendFovAngle;
            extendedSight = true;
            animator.SetBool("IdleBattle", true); // 경계 상태

            yield return new WaitForSeconds(extendFovTime);

            fov.viewRadius = BASE_FOV_RADIUS; // 시야 복귀
            fov.viewAngle = BASE_FOV_ANGLE;
            extendedSight = false;
            animator.SetBool("IdleBattle", false); // 경계 상태 종료
        }

        public void ExtendSight()
        {
            if (extendSightCo != null)
                StopCoroutine(extendSightCo); // 이미 시야증가가 적용중이면 이전것을 종료
            extendSightCo = StartCoroutine(SightCo()); // 시야증가 새로 시작
        }

        public void EndAttack() // 공격 애니메이션의 끝에 호출, "공격중" 플래그를 끄기 위함
        {
            whileAttack = false;
        }

        public Vector3 GetRandomPosInPatrolRadius()
        {
            Vector3 target = Vector3.zero;
            target.z += Random.Range(-patrolRadius, patrolRadius);
            target.x += Random.Range(-patrolRadius, patrolRadius);
            if (target.magnitude > patrolRadius)
            {
                target = target.normalized;
                target *= patrolRadius;
            }

            return spawnPoint + target;
        }

        public void Die()
        {
            Destroy(this.gameObject);
        }
    }

    public enum EMonsterState // 몬스터의 현재 상태를 나타내기 위한 열거형
    {
        Idle,
        Patrol,
        ChasePlayer,
        BaseAttack,
        Runaway,
        Dead,
        Stiff
    }

    public enum EMonsterType
    {
        General_Melee,
        General_Ranged
    }
}