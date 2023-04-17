using UnityEngine;
using UnityEngine.AI;

namespace Monsters.FSM
{
    public class State_Patrol : State
    {
        public override void Enter(Monster monster)
        {
            base.Enter(monster);
            monster.state = EMonsterState.Patrol;

            Vector3 pos = monster.GetRandomPosInPatrolRadius();
            monster.nav.SetDestination(pos);
            
            monster.animator.SetBool("Walk", true); // 순찰 애니메이션
        }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            // https://answers.unity.com/questions/324589/how-can-i-tell-when-a-navmesh-has-reached-its-dest.html

            if (!monster.nav.pathPending)
            {
                if (monster.nav.remainingDistance < 0.1f)
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


            if (monster.nav.velocity.sqrMagnitude > 3.5f) // 정상적으로 움직이고 있으면 0으로 초기화
            {
                monster.patrolRaceElapsedTime = 0;
            }
            else
            {
                monster.patrolRaceElapsedTime += Time.deltaTime;
                if (monster.patrolRaceElapsedTime > 1.1f) // 많은 유닛이 경로가 서로 겹쳐서 계속 부비는걸 방지
                {
                    Debug.Log("stop!!!");
                    monster.patrolRaceElapsedTime = 0;
                    monster.fsm.ChangeState(EMonsterState.Idle);
                    return;
                }
            }
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
            monster.animator.SetBool("Walk", false);
            monster.nav.ResetPath();
        }
    }
}