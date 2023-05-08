using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.FOV
{
    public class MonsterFOV : MonoBehaviour
    {
        // https://nicotina04.tistory.com/197
        public float viewRadius;
        [Range(0, 360)] public float viewAngle;

        public LayerMask targetMask, obstacleMask;

        [HideInInspector] public Monster monster;

        public bool wallException = true;

        private void Awake()
        {
            monster = GetComponent<Monster>();
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

                    if (Vector3.Angle(transform.forward, dirToTarget) <= viewAngle / 2)
                    {
                        float dstToTarget = Vector3.Distance(transform.position, target.transform.position);
                        monster.playerDist = dstToTarget;

                        if (wallException)
                        {
                            if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                            {
                                monster.player = _player;
                                monster.playerInSight = true;
                                foundPlayer = true;
                                monster.ExtendSight();
                            }
                        }
                        else
                        {
                            monster.player = _player;
                            monster.playerInSight = true;
                            foundPlayer = true;
                            monster.ExtendSight();
                        }
                    }
                }
            }

            if (!foundPlayer)
            {
                monster.player = null;
                monster.playerInSight = false;
                monster.playerDist = -1f;
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
}