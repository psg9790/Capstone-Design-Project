using System.Collections;
using System.Collections.Generic;
using System.Net;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    // components
    [HideInInspector] public NavMeshAgent nav;
    [HideInInspector] public Animator animator;

    // behavior
    [HideInInspector] public MonsterStateMachine fsm;
    [ReadOnly] public EMonsterState state;

    // spawner & player
    [HideInInspector] public MonsterSpawner spawner;
    [HideInInspector] public Vector3 patrolPoint;
    [HideInInspector] public Player player;
    [ReadOnly] public bool playerInSight = false;
    
    // battle
    [SerializeField] public float attackRange = 1.75f;
    [ReadOnly] public bool whileAttack = false;

    // fov
    [HideInInspector] public MonsterFOV fov;
    [HideInInspector] public float BASE_FOV_RADIUS;
    [HideInInspector] public float BASE_FOV_ANGLE;
    [BoxGroup("FOV")] public float extendFovRadius_multi = 2f;
    [BoxGroup("FOV")] [Range(0, 360)] public float extendFovAngle = 360f;
    [BoxGroup("FOV")] public float extendFovTime = 4f;
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
        fsm = new MonsterStateMachine(this);
        if (TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            nav = agent;
            nav.updateRotation = false;
        }
        else
        {
            Debug.LogError("no navmeshagent assigned in " + this.gameObject.name);
        }

        if (TryGetComponent<Animator>(out Animator anim))
        {
            animator = anim;
        }
        else
        {
            Debug.LogError("no animator assigned in " + this.gameObject.name);
        }

        if (TryGetComponent<MonsterFOV>(out MonsterFOV _fov))
        {
            fov = _fov;
            BASE_FOV_RADIUS = fov.viewRadius;
            BASE_FOV_ANGLE = fov.viewAngle;
        }
        else
        {
            UnityEngine.Debug.LogError("no fov assigned in " + this.gameObject.name);
        }

        fsm.ChangeState(new MonsterState_Idle(this));
    }

    protected virtual void OnUpdate()
    {
        // 현재 state 행동 
        fsm.Execute();

        // nav용 rotation
        NavRotation();

        // 시야에 플레이어가 있는지 갱신
        fov.FindVisiblePlayer();

        // 기본 행동
        switch (state)
        {
            case EMonsterState.Idle: // 대기 상태
                if (playerInSight)
                {
                    // https://forum.unity.com/threads/getting-the-distance-in-nav-mesh.315846/
                    NavMeshPath path = new NavMeshPath();
                    nav.CalculatePath(player.transform.position, path);
                    float dist = 0f;
                    if ((path.status != NavMeshPathStatus.PathInvalid))
                    {
                        for (int i = 1; i < path.corners.Length; ++i)
                        {
                            dist += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                        }
                    }
                    
                    if (dist > attackRange)
                    {
                        fsm.ChangeState(new MonsterState_ChasePlayer(this));
                    }
                    else
                    {
                        // 공격 쿨타임?
                        fsm.ChangeState(new MonsterState_BaseAttack(this));
                    }
                }
                
                idleElapsedTime += Time.deltaTime;
                if (idleElapsedTime > idleToPatrolTime) // 일정 시간 대기하면 순찰
                    fsm.ChangeState(new MonsterState_Patrol(this));
                // if (playerInSight) // 플레이어 발견시 추적
                //     fsm.ChangeState(new MonsterState_ChasePlayer(this));
                break;

            case EMonsterState.Patrol: // 순찰 상태
                if (playerInSight) // 플레이어 발견시 추적
                    fsm.ChangeState(new MonsterState_ChasePlayer(this));
                
                catchPatrolRaceCondition += Time.deltaTime;
                if (catchPatrolRaceCondition > 3)
                {
                    Debug.Log("stop!!!");
                    catchPatrolRaceCondition = 0;
                    fsm.ChangeState(new MonsterState_Idle(this));
                }
                if (nav.velocity.sqrMagnitude > 3.5f)
                    catchPatrolRaceCondition = 0;

                break;

            case EMonsterState.ChasePlayer: // 추적 상태
                if (!nav.pathPending)
                {
                    if (nav.remainingDistance < attackRange)
                    {
                        // attack here
                        fsm.ChangeState(new MonsterState_BaseAttack(this));
                    }
                }

                if (!playerInSight) // 플레이어 놓쳤을 시 대기
                    fsm.ChangeState(new MonsterState_Idle(this));
                break;

            
            case EMonsterState.BaseAttack: // 기본 공격 수행
                if (!whileAttack)
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
    private Coroutine extendSightCo; // 하나의 시야증가만을 유지하기 위해 변수 하나로 통제
    private IEnumerator SightCo()
    {
        fov.viewRadius = BASE_FOV_RADIUS * extendFovRadius_multi; // 시야를 증가시킴
        fov.viewAngle = extendFovAngle;
        extendedSight = true;
        animator.SetBool("IdleBattle" , true);  // 전투 상태를 나타냄

        yield return new WaitForSeconds(extendFovTime); // 설정한 초만큼 유지시키고

        fov.viewRadius = BASE_FOV_RADIUS; // 다시 원래대로 되돌림
        fov.viewAngle = BASE_FOV_ANGLE;
        extendedSight = false;
        animator.SetBool("IdleBattle", false);  // 전투 상태를 해제함
    }
    public void ExtendSight()
    {
        if (extendSightCo != null)
            StopCoroutine(extendSightCo); // 이미 시야증가를 돌린적이 있으면 취소시킴
        extendSightCo = StartCoroutine(SightCo()); // 시야증가 코루틴 시작
    }

    /// <summary>
    /// 시야 증가 코루틴 끝
    /// </summary>

    public void EndAttack()
    {
        whileAttack = false;
    }
    public virtual void OnBaseAttack()
    {
    }
}

public enum EMonsterState
{
    Idle,
    Patrol,
    ChasePlayer,
    BaseAttack,
    IdleBattle
}