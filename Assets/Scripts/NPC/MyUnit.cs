using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyUnit : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 destination;
    private int walkableLayer;
    [SerializeField] public Transform character;
    [SerializeField] public float rotationSpeed;
    private bool isMove = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        walkableLayer = 1 << LayerMask.NameToLayer("Walkable");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
            {
                destination = hitInfo.point;
            }
        }

        LookAt();

    }

    void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
        this.destination = destination;
        isMove = true;
    }


    void LookAt()
    {
        if (isMove)
        {
            bool isAlived = agent.velocity.sqrMagnitude >= 0.1f * 0.1f && agent.remainingDistance <= 0.1f;
            bool isMoving = agent.desiredVelocity.sqrMagnitude >= 0.1f * 0.1f;

            if (isAlived)
            {
                isMove = false;
            }
            else if (isMoving)
            {
                Vector3 direction = agent.desiredVelocity;
                direction.Set(direction.x, 0f, direction.z); // 경사진 땅 위에 올라갔을 때 회전 방지
                Quaternion targetAngle = Quaternion.LookRotation(direction);
                character.rotation = Quaternion.Slerp(character.rotation, targetAngle, rotationSpeed * Time.deltaTime);
            }
        }
    }
}