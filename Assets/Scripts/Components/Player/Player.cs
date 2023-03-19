using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    // event for camera
    // [HideInInspector] public UnityEvent playerMoveEvent;
    
    // state
    [Sirenix.OdinInspector.ReadOnly] 
    public PlayerState state = PlayerState.Idle;
    
    // move
    private NavMeshAgent nav;
    private Vector3 moveTarget;
    
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // 움직이는 상태이면
        if (state == PlayerState.Move)
        {
            Vector3 dist = moveTarget - transform.position;
            if (dist.magnitude <= 0.1f)
            {
                state = PlayerState.Idle;
            }
            // playerMoveEvent.Invoke();
        }
    }

    public void Move(Vector3 pos)
    {
        if (state != PlayerState.Death && 
            state != PlayerState.Cc && 
            state != PlayerState.Attack)
        {
            state = PlayerState.Move;
            nav.SetDestination(pos);
            moveTarget = pos;
        }
    }
}

public enum PlayerState
{
    Idle, 
    Move,
    Dash,
    Attack,
    Interact,
    Cc,
    Death
}
