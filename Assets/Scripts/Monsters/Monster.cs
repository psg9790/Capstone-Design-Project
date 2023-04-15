// 몬스터 최상위 클래스
// 앞으로 구현할 세부 몬스터들은 이 클래스를 상속받아서 구현하게 될 것

using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Monsters.FOV;
using Monsters.FSM;
using Monsters.Skill;

namespace Monsters
{
    public class Monster : MonoBehaviour
    {
        public EMonsterType monsterType; // 상태 분기에 사용
        [Required] public Collider bodyCollider; // 사망 처리 사용
        [Required] public List<SkinnedMeshRenderer> renders = new List<SkinnedMeshRenderer>(); // 피격 하이라이트 사용

        // 체력, 스탯 관련
        [HideInInspector] public Heart heart;

        // components
        [HideInInspector] public NavMeshAgent nav; // 컴포넌트 미리 추가 필요
        [HideInInspector] public Animator animator; // 컴포넌트 미리 추가 필요
        [HideInInspector] public MonsterFOV fov; // 시야, 컴포넌트 미리 추가 필요
        [HideInInspector] public SkillSet skillset; // 몬스터별 스킬 구현 사용

        // behavior
        public StateMachine fsm; // 상태의 변경을 관리할 장치, 상태 변경 시 이전 상태의 Exit() 실행 후 새로운 상태의 Enter()를 실행시켜 줌
        [ReadOnly] public EMonsterState state; // 몬스터의 현재 상태를 표시할 열거형 변수, 현재 상태가 무엇인지 검사하는데 쓰임

        // spawner & player
        [HideInInspector] public Player player; // 플레이어가 시야에 있을 시 플레이어 정보를 넣어줄 변수
        [HideInInspector] public Vector3 spawnPoint; // 몬스터가 스폰된 위치
        [HideInInspector] public float patrolRadius; // 몬스터가 스폰 위치로부터 순찰할 반지름 거리

        // battle
        [BoxGroup("Battle")] public float attackRange = 2.2f; // 몬스터의 공격 사정거리
        [BoxGroup("Battle")] [ReadOnly] public bool whileEngage; // 공격 중플래그
        [BoxGroup("Battle")] [ReadOnly] public bool whileStiff; // 경직 중 플래그
        [BoxGroup("Battle")] [HideInInspector] public float afterDeadTime = 1.25f;
        [BoxGroup("Battle")] [ReadOnly] public float afterDeadElapsed = 0f;

        // fov
        [HideInInspector] public float BASE_FOV_RADIUS; // 몬스터의 기본 시야 범위를 저장 -> 복구에 사용
        [HideInInspector] public float BASE_FOV_ANGLE; // 몬스터의 기본 시야각을 저장 -> 복구에 사용
        [BoxGroup("FOV")]
        public float extendFovRadius_multi = 2f; // 흥분상태의 확장 시야 범위. 기본시야에 곱해진다. 인스펙터에서 수정할 것.
        [BoxGroup("FOV")] [Range(0, 360)]
        public float extendFovAngle = 360f; // 흥분상태의 확장 시야각. 이 값으로 덮어씌워짐. 인스펙터에서 수정.
        [BoxGroup("FOV")] public float extendFovTime = 4f; // 흥분 상태에서 기본상태로 전환될 시간
        [BoxGroup("FOV")] [ReadOnly] public bool extendedSight; // 흥분 중 플래그
        [BoxGroup("FOV")] [ReadOnly] public bool playerInSight; // 플레이어 탐색에 있어서 null체크를 줄이기 위해 플래그로 관리
        [BoxGroup("FOV")] [ReadOnly] public float playerDist = -1f; // 플레이어가 시야에 있으면 그 거리 갱신
        [BoxGroup("FOV")] public float runawayDistance = 7f; // (원거리)몬스터가 플레이어로부터 도망가기 시작할 거리

        // infos
        [HideInInspector] public float idleElapsedTime; // idle 경과시간
        [HideInInspector] public float idleEndTime = 4f; // idle 목표시간
        [HideInInspector] public float patrolRaceElapsedTime; // 경쟁상태 경과 시간


        public void Init(Vector3 pos, float range) // 스폰시 스폰 위치와 탐색 반경 설정
        {
            this.transform.position = pos;
            spawnPoint = pos;
            patrolRadius = range;
        }

        void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        void Update()
        {
            OnUpdate();
        }

        protected virtual void OnAwake() // 컴포넌트 초기화 담당
        {
            if (StateLists.Instance == null) // monster에서 사용할 state list가 존재하지 않으면 생성
            {
                GameObject stateGameObject = new GameObject("Monster_StateLists");
                stateGameObject.AddComponent<StateLists>();
            }

            if (TryGetComponent<Heart>(out Heart hrt))
            {
                heart = hrt;
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

            if (TryGetComponent<MonsterFOV>(out MonsterFOV mf))
            {
                fov = mf;
                BASE_FOV_RADIUS = fov.viewRadius;
                BASE_FOV_ANGLE = fov.viewAngle;
            }
            else
            {
                Debug.LogError(this.gameObject.name + " 몬스터에 \"MonsterFOV\" 컴포넌트가 없습니다.");
            }

            if (TryGetComponent<SkillSet>(out SkillSet ss))
            {
                skillset = ss;
            }
            else
            {
                Debug.LogError(this.gameObject.name + " 몬스터에 \"SkillSet\" 계열 컴포넌트가 없습니다.");
            }

            mpb = new MaterialPropertyBlock();
            mpb.SetColor(Shader.PropertyToID("_Color"), Color.red);
        }

        protected virtual void OnStart()
        {
            fsm = new StateMachine(this);
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
        }

        public void DoPossibleEngage()
        {
            skillset.DoPossibleEngage();
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
            // animator.SetBool("IdleBattle", true); // 경계 상태

            yield return new WaitForSeconds(extendFovTime);

            fov.viewRadius = BASE_FOV_RADIUS; // 시야 복귀
            fov.viewAngle = BASE_FOV_ANGLE;
            extendedSight = false;
            // animator.SetBool("IdleBattle", false); // 경계 상태 종료
        }

        public void ExtendSight()
        {
            if (extendSightCo != null)
                StopCoroutine(extendSightCo); // 이미 시야증가가 적용중이면 이전것을 종료
            extendSightCo = StartCoroutine(SightCo()); // 시야증가 새로 시작
        }

        public void ForceCC_Stiff_Event()
        {
            // if (!fsm.CheckCurState(EMonsterState.Stiff))
            // {
            //     // stiffTime = power;
            //     fsm.ChangeState(EMonsterState.Stiff);
            // }
            // else
            // {
            // if (stiffTime - stiffElapsed < power) // 잔여경직시간 < 새 경직시간
            // {
            //     stiffTime = power;
            fsm.ChangeState(EMonsterState.Stiff);
            // }
            // }
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

        MaterialPropertyBlock mpb;
        private Coroutine hitColorCo;

        private void SetSMRPropertyBlocks(MaterialPropertyBlock mpb)
        {
            for (int i = 0; i < renders.Count; i++)
            {
                renders[i].SetPropertyBlock(mpb);
            }
        }
        private IEnumerator hitColoring(float duration)
        {
            // https://cacodemon.tistory.com/entry/material-%EA%B3%BC-sharedMaterial-%EA%B7%B8%EB%A6%AC%EA%B3%A0-Material-Property-Block
            // smesh.SetPropertyBlock(mpb);
            SetSMRPropertyBlocks(mpb);
            yield return new WaitForSeconds(duration);
            // smesh.SetPropertyBlock(null);
            SetSMRPropertyBlocks(null);
        }

        public void OnHit_Event(float duration, Vector3 dir)
        {
            if (renders.Count == 0)
            {
                Debug.Log("Monster 스크립트에 skinned mesh renderer를 등록해주세요.");
                return;
            }

            if (hitColorCo != null)
            {
                StopCoroutine(hitColorCo);
            }

            fsm.ChangeState(EMonsterState.Idle);
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            hitColorCo = StartCoroutine(hitColoring(duration));
        }

        public void OnDeath_Event()
        {
            fsm.ChangeState(EMonsterState.Die);
        }

        void EndStiff()
        {
            whileStiff = false;
        }

        public void Eliminate()
        {
            Destroy(this.gameObject);
        }
    }

    public enum EMonsterState // 몬스터의 현재 상태를 나타내기 위한 열거형
    {
        Idle,
        Patrol,
        ChasePlayer,
        Runaway,
        Die,
        Stiff,
        Engage
    }

    public enum EMonsterType
    {
        General_Melee,
        General_Ranged
    }
}