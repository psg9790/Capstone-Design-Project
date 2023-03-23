using System.Collections;
using System.Collections.Generic;
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
    
    // target
    [HideInInspector] public MonsterSpawner spawner; 
    [HideInInspector] public Vector3 patrolPoint;
    [HideInInspector] public Player player;
    [ReadOnly] public bool playerInSight = false;
    
    // infos
    [ReadOnly] public float idleElapsedTime = 0f;
    [SerializeField] public float idleToPatrolTime = 4f;
    [SerializeField] public float attackRange = 1.75f;
    
    // fov
    [HideInInspector] public MonsterFOV fov;
    
    
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
        switch(state)
        {
            case EMonsterState.Idle:
                idleElapsedTime += Time.deltaTime;
                if (idleElapsedTime > idleToPatrolTime)
                    fsm.ChangeState(new MonsterState_Patrol(this));
                if(playerInSight)
                    fsm.ChangeState(new MonsterState_ChasePlayer(this));
                break;
            case EMonsterState.Patrol:
                if(playerInSight)
                    fsm.ChangeState(new MonsterState_ChasePlayer(this));
                break;
            case EMonsterState.ChasePlayer:
                if(!nav.pathPending)
                    if (nav.remainingDistance < attackRange)
                    {
                        // fsm.ChangeState(new MonsterState_Idle(this));
                        nav.ResetPath();
                    }

                if (!playerInSight)
                {
                    fsm.ChangeState(new MonsterState_Idle(this));
                }
                break;
        }
    }
    
    void NavRotation()
    {
        if (!nav.hasPath)
            return;
        
        Vector2 forward = new Vector2(transform.position.z, transform.position.x);
        Vector2 steeringTarget = new Vector2(nav.steeringTarget.z, nav.steeringTarget.x);
    
        //방향을 구한 뒤, 역함수로 각을 구한다.
        Vector2 dir = steeringTarget - forward;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    
        //방향 적용
        transform.eulerAngles = Vector3.up * angle;
    }

}

public enum EMonsterState
{
    Idle,
    Patrol,
    ChasePlayer,
    R2B
}