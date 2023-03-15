using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public PlayerState state;
    NavMeshAgent agent;
    Vector3 movePos;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void OnDestroy()
    {
    }

    void Update()
    {
        // 이동
        if (state == PlayerState.Move)
        {
            Vector3 dist = movePos - transform.position;
            if (dist.magnitude < 0.1f)
            {
                state = PlayerState.Idle;
            }
            agent.SetDestination(movePos);
        }
    }
    public void MovePlayer(Vector3 target)
    {
        movePos = target;
        state = PlayerState.Move;
    }
}
public enum PlayerState
{
    Idle,
    Move
}
