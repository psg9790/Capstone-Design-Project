using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public UnityEvent playerMoveEvent;
    private NavMeshAgent nav;

    public PlayerBehavior behavior = PlayerBehavior.Idle;
    private Vector3 moveTarget;
    
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // 움직이는 상태이면
        if (behavior.CompareTo(PlayerBehavior.Move) == 0)
        {
            Vector3 dist = moveTarget - transform.position;
            if (dist.magnitude <= 0.1f)
            {
                behavior = PlayerBehavior.Idle;
            }
            playerMoveEvent.Invoke();
        }
    }

    public void Move(Vector3 pos)
    {
        
        behavior = PlayerBehavior.Move;
        nav.SetDestination(pos);
        moveTarget = pos;
    }
}

public enum PlayerBehavior
{
    Idle, 
    Move,
    Dash,
    Attack,
    Interact,
    Cc,
    Death
}
