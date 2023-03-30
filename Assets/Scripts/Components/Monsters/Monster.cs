// 몬스터 최상위 클래스
// 앞으로 구현할 세부 몬스터들은 이 클래스를 상속받아서 구현하게 될 것

using System.Collections;
using System.Collections.Generic;
using System.Net;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    // components
    [HideInInspector] public NavMeshAgent nav; // 컴포넌트 미리 추가 필요
    [HideInInspector] public Animator animator; // 컴포넌트 미리 추가 필요
    [HideInInspector] public MonsterFOV fov; // 컴포넌트 미리 추가 필요
    [HideInInspector] public MonsterSkillArchive skills;
    
    // behavior
    [HideInInspector] public MonsterStateMachine fsm; // 상태의 변경을 관리할 장치, 상태 변경 시에 이전 상태의 Exit(), 새로운 상태의 Enter()를 실행시켜줌
    [ReadOnly] public EMonsterState state; // 몬스터의 현재 상태를 나타내줄 열거형 변수, 현재 상태가 무엇인지 검사하는데 쓰임

    // spawner & player
    // [HideInInspector] public MonsterSpawner spawner; // 몬스터가 태어난 스포너의 정보. 이걸 가지고 있어야 스포너 주변 좌표를 가져올 수 있다.
    [HideInInspector] public Player player; // 플레이어를 찾았을 시 플레이어 정보를 넣어줄 변수
    [ReadOnly] public bool playerInSight = false; // 플레이어 정보 저장에 있어서 null체크를 줄이기 위해 bool값으로 따로 관리
    [HideInInspector] public Vector3 spawnPoint;
    [HideInInspector] public float patrolRadius;
    
    // battle
    [BoxGroup("Battle")] [SerializeField] public float attackRange = 1.75f; // 이 몬스터의 공격 사정거리
    [BoxGroup("Battle")] [ReadOnly] public bool whileAttack = false; // 공격 중에 다른 행동을 막기 위한 플래그

    // fov
    [HideInInspector] public float BASE_FOV_RADIUS; // 몬스터의 기본 시야 범위를 저장하기 위한 변수. 시야 확장 후 복귀에 사용
    [HideInInspector] public float BASE_FOV_ANGLE; // 몬스터의 기본 시야각을 저장하기 위한 변수. 시야 확장 후 복귀에 사용
    [BoxGroup("FOV")] public float extendFovRadius_multi = 2f; // 몬스터의 확장 시야 범위를 결정할 변수. 기본 시야 범위에 곱해진다. 인스펙터에서 수정할 것.
    [BoxGroup("FOV")] [Range(0, 360)] public float extendFovAngle = 360f; // 몬스터의 확장 시야각을 결정할 변수. 이 각으로 덮어씌워진다.
    [BoxGroup("FOV")] public float extendFovTime = 4f; // 확장된 시야 범위에서 기본 시야 범위로 돌아갈 시간
    [BoxGroup("FOV")] public bool extendedSight = false;

    // infos
    [FoldoutGroup("Idle->Patrol info")] [ReadOnly]
    public float idleElapsedTime = 0f;

    [FoldoutGroup("Idle->Patrol info")] [ReadOnly]
    public float idleToPatrolTime = 4f;

    [FoldoutGroup("Patrol Race Condition Control")] [ReadOnly]
    public float catchPatrolRaceCondition = 0;

    // [FoldoutGroup("Patrol Race Condition Control")] [ReadOnly]
    // public float velocity;

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
            Debug.LogError(this.gameObject.name+" 몬스터에 \"MonsterSkillArchive\" 컴포넌트가 없습니다.");
        }

        fsm = new MonsterStateMachine(this);
        fsm.ChangeState(new MonsterState_Idle(this));
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
                if (playerInSight) // 플레이어가 시야에 들어오면
                {
                    // https://forum.unity.com/threads/getting-the-distance-in-nav-mesh.315846/
                    NavMeshPath path = new NavMeshPath();
                    nav.CalculatePath(player.transform.position, path);
                    float dist = 0f;
                    if ((path.status != NavMeshPathStatus.PathInvalid))
                    {
                        for (int i = 1; i < path.corners.Length; ++i)
                        {
                            dist += Vector3.Distance(path.corners[i - 1], path.corners[i]); // 타깃까지의 거리를 계산
                        }

                        if (dist > attackRange) // 타깃이 공격 사정거리보다 멀면
                        {
                            fsm.ChangeState(new MonsterState_ChasePlayer(this));
                        }
                        else // 타깃이 공격 사정거리 안이면
                        {
                            // 공격 쿨타임 추가?
                            fsm.ChangeState(new MonsterState_BaseAttack(this));
                        }
                    }
                }

                idleElapsedTime += Time.deltaTime;
                if (idleElapsedTime > idleToPatrolTime) // 정해진 시간을 대기하면
                    fsm.ChangeState(new MonsterState_Patrol(this));
                break;

            case EMonsterState.Patrol: // 순찰 상태
                if (playerInSight) // 플레이어 발견시
                    fsm.ChangeState(new MonsterState_ChasePlayer(this));

                catchPatrolRaceCondition += Time.deltaTime; // 정상 속도로 움직일 시 계속 0으로 초기화됨.
                if (catchPatrolRaceCondition > 3) // 너무 오래 일정속도 이하로 있으면
                {
                    Debug.Log("stop!!!");
                    catchPatrolRaceCondition = 0;
                    fsm.ChangeState(new MonsterState_Idle(this));
                }

                if (nav.velocity.sqrMagnitude > 3.5f) // 일정 속도 이상으로 움직이고 있으면 0으로 초기화
                    catchPatrolRaceCondition = 0;

                break;

            case EMonsterState.ChasePlayer: // 추적 상태
                if (!nav.pathPending) // 네비게이션이 경로 탐색을 완료했고
                {
                    if (nav.remainingDistance < attackRange) // 플레이어가 공격 사정거리 안에 들어왔을 때
                    {
                        fsm.ChangeState(new MonsterState_BaseAttack(this));
                    }
                }

                if (!playerInSight) // 플레이어를 시야에서 놓쳤을 시
                    fsm.ChangeState(new MonsterState_Idle(this));
                break;


            case EMonsterState.BaseAttack: // 기본 공격 수행 중
                if (!whileAttack) // 공격중 플래그가 꺼지면
                    fsm.ChangeState(new MonsterState_Idle(this));
                break;
        }
    }

    void NavRotation()
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

    private IEnumerator SightCo() // 위 extendSightCo 변수에 들어갈 코루틴 
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
        Vector3 target = spawnPoint;
        // target.y = this.transform.position.y;
        target.z += Random.Range(-patrolRadius, patrolRadius);
        target.x += Random.Range(-patrolRadius, patrolRadius);
        if (target.magnitude > patrolRadius)
        {
            target = target.normalized;
            target *= 3;
        }

        return this.transform.position + target;
    }
}

public enum EMonsterState // 몬스터의 현재 상태를 나타내기 위한 열거형
{
    Idle,
    Patrol,
    ChasePlayer,
    BaseAttack,
    IdleBattle
}