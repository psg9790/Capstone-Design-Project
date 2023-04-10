using UnityEngine;
using UnityEngine.AI;

namespace Monsters
{
    public class MonsterState_Patrol : MonsterState
    {
        public override void Enter(Monster monster)
        {
            base.Enter(monster);
            monster.state = EMonsterState.Patrol;

            // NavMeshPath path = new NavMeshPath();
            // while (true)
            // {
            //     /*
            //      몬스터가 스폰포인트에서 랜덤 좌표를 가져오는데
            //      그 좌표가 navigation을 사용하는데 적절하지 않은 좌표일 수 있어서 (Not Walkable)
            //      적절한 좌표를 가져올 때까지 지점 탐색을 반복함
            //      */
            //     Vector3 patrolPoint = monster.GetRandomPosInPatrolRadius();
            //     if (monster.nav.CalculatePath(patrolPoint, path))
            //     {
            //         monster.nav.SetDestination(patrolPoint);
            //         break;
            //     }
            // }

            Vector3 pos = monster.GetRandomPosInPatrolRadius();
            monster.nav.SetDestination(pos);
            // Debug.DrawRay(monster.transform.position + Vector3.up * 0.5f, pos - monster.transform.position, Color.red, 5f);

            monster.animator.SetBool("Patrol", true); // 순찰 애니메이션
        }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            // https://answers.unity.com/questions/324589/how-can-i-tell-when-a-navmesh-has-reached-its-dest.html

            if (!monster.nav.pathPending)
            {
                if (monster.nav.remainingDistance < monster.nav.stoppingDistance)
                {
                    if (!monster.nav.hasPath || monster.nav.velocity.sqrMagnitude == 0f)
                    {
                        monster.fsm.ChangeState(EMonsterState.Idle);
                        return;
                    }
                }
            }
            
            if (monster.playerInSight) // 플레이어 발견시
            {
                monster.fsm.ChangeState(EMonsterState.ChasePlayer);
                return;
            }

            if (monster.catchPatrolRaceCondition > 1.1f) // 너무 오래 일정속도 이하로 있으면
            {
                Debug.Log("stop!!!");
                monster.catchPatrolRaceCondition = 0;
                monster.fsm.ChangeState(EMonsterState.Idle);
                return;
            }

            if (monster.nav.velocity.sqrMagnitude > 3.5f) // 일정 속도 이상으로 움직이고 있으면 0으로 초기화
                monster.catchPatrolRaceCondition = 0;
            else
                monster.catchPatrolRaceCondition += Time.deltaTime; // 정상 속도로 움직일 시 계속 0으로 초기화됨.
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
            monster.animator.SetBool("Patrol", false);
            monster.nav.ResetPath();
        }
    }
}