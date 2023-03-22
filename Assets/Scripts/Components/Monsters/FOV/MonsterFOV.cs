using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFOV : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)] public float viewAngle;

    public LayerMask targetMask, obstacleMask;

    public Player visiblePlayer = null;


    private void Update()
    {
        FindVisiblePlayer();
    }

    public void FindVisiblePlayer()
    {
        bool foundPlayer = false;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position,
            viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            if (targetsInViewRadius[i].TryGetComponent<Player>(out Player _player))
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.transform.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                    {
                        visiblePlayer = _player;
                        foundPlayer = true;
                    }
                }
            }
        }

        if (!foundPlayer)
        {
            visiblePlayer = null;
        }
    }

    public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 
            0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
    }
}
