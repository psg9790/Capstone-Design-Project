using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    // behavior
    [HideInInspector] public NavMeshAgent nav;
    [HideInInspector] public Animator animator;
    [HideInInspector] public MonsterStateMachine fsm;
    [ReadOnly] public EMonsterState _state;
    
    // target
    [HideInInspector] public Player player;
    [HideInInspector] public Vector3 patrolPoint;
    
    // infos
    [ShowInInspector] 
    [ReadOnly]
    private bool playerInSight = false;

    
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
        fsm.ChangeState(new MonsterState_Idle(this));
        if (TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            nav = agent;
        }
        else
        {
            Debug.LogError("no nav in " + this.gameObject.name);
        }
    }

    protected virtual void OnUpdate()
    {
        // 현재 state 행동 
        fsm.Execute();
        
        // 시야에 플레이어가 있는지 갱신
        // PlayerInSight();
        
        // 기본 행동
        switch(_state)
        {
            case EMonsterState.Idle:
                if(playerInSight)
                    fsm.ChangeState(new MonsterState_ChasePlayer(this));
                break;
            case EMonsterState.Patrol:
                if(playerInSight)
                    fsm.ChangeState(new MonsterState_ChasePlayer(this));
                break;
            case EMonsterState.ChasePlayer:
                if (!playerInSight)
                    fsm.ChangeState(new MonsterState_Idle(this));
                break;
        }
    }

    [Button]
    protected virtual void PlayerInSight()
    {
        playerInSight = !playerInSight;
    }
}

public enum EMonsterState
{
    Idle,
    Patrol,
    ChasePlayer,
    R2B
}