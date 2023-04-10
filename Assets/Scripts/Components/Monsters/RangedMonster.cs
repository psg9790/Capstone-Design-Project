using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Monsters
{

    public class RangedMonster : Monster
    {
        public float startRunawayDistance = 3f;


        protected override void OnAwake()
        {
            base.OnAwake();
        }

        protected override void OnUpdate()
        {
            // 현재 state행동 update
            fsm.Execute();

            // nav용 rotation
            NavRotation();

            // 시야에 플레이어가 있는지 갱신
            fov.FindVisiblePlayer();

            // 기본 행동
            switch (state)
            {
                case EMonsterState.Idle: // 대기 상태
                    if (playerInSight) // 플레이어가 시야에 들어오면
                    {
                        // https://forum.unity.com/threads/getting-the-distance-in-nav-mesh.315846/
                        if (playerDist < startRunawayDistance)
                        {
                            fsm.ChangeState(EMonsterState.Runaway);
                        }
                        else if (playerDist > attackRange) // 타깃이 공격 사정거리보다 멀면
                        {
                            fsm.ChangeState(EMonsterState.ChasePlayer);
                        }
                        else // 타깃이 공격 사정거리 안이면
                        {
                            // 공격 쿨타임 추가?
                            fsm.ChangeState(EMonsterState.BaseAttack);
                        }
                    }

                    // idleElapsedTime += Time.deltaTime;
                    // if (idleElapsedTime > idleToPatrolTime) // 정해진 시간을 대기하면
                    // fsm.ChangeState(new MonsterState_Patrol(this));
                    break;

                case EMonsterState.Patrol: // 순찰 상태
                    if (playerInSight) // 플레이어 발견시
                    {
                        if (playerDist < startRunawayDistance)
                        {
                            fsm.ChangeState(EMonsterState.Runaway);
                        }
                        else if (playerDist > attackRange)
                        {
                            fsm.ChangeState(EMonsterState.ChasePlayer);
                        }
                        else
                        {
                            fsm.ChangeState(EMonsterState.BaseAttack);
                        }
                    }

                    catchPatrolRaceCondition += Time.deltaTime; // 정상 속도로 움직일 시 계속 0으로 초기화됨.
                    if (catchPatrolRaceCondition > 1.1f) // 너무 오래 일정속도 이하로 있으면
                    {
                        Debug.Log("stop!!!");
                        catchPatrolRaceCondition = 0;
                        fsm.ChangeState(EMonsterState.Idle);
                    }

                    if (nav.velocity.sqrMagnitude > 3.5f) // 일정 속도 이상으로 움직이고 있으면 0으로 초기화
                        catchPatrolRaceCondition = 0;

                    break;

                case EMonsterState.ChasePlayer: // 추적 상태
                    if (playerInSight) // 네비게이션이 경로 탐색을 완료했고
                    {
                        if (playerDist < startRunawayDistance)
                        {
                            fsm.ChangeState(EMonsterState.Runaway);
                        }
                        else if (playerDist < attackRange) // 플레이어가 공격 사정거리 안에 들어왔을 때
                        {
                            fsm.ChangeState(EMonsterState.BaseAttack);
                        }
                    }
                    else // 플레이어를 시야에서 놓쳤을 시
                    {
                        fsm.ChangeState(EMonsterState.Idle);
                    }

                    break;


                case EMonsterState.BaseAttack: // 기본 공격 수행 중
                    if (!whileAttack) // "공격중" 플래그가 꺼지면 (애니메이션 마지막에 이벤트로 끔)
                        fsm.ChangeState(EMonsterState.Idle);
                    break;

                case EMonsterState.Runaway:
                    if (playerInSight)
                    {
                        if (playerDist > startRunawayDistance)
                        {
                            fsm.ChangeState(EMonsterState.Idle);
                        }
                    }
                    else
                    {
                        fsm.ChangeState(EMonsterState.Idle);
                    }

                    break;
            }
        }
    }
}