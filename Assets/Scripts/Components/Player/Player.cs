using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    // state
    [Sirenix.OdinInspector.ReadOnly] 
    public PlayerState state = PlayerState.Idle;
    
    // move
    private NavMeshAgent nav;
    private Vector3 moveTarget;
    
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.updateRotation = false;
    }

    void Update()
    {
        NavRotation();
        // 움직이는 상태이면
        if (state == PlayerState.Move)
        {
            Vector3 dist = moveTarget - transform.position;
            if (dist.magnitude <= 0.1f)
            {
                state = PlayerState.Idle;
            }
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
