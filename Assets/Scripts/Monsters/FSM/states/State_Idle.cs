using UnityEngine;
using Random = UnityEngine.Random;

namespace Monsters.FSM
{
    public class State_Idle : State
    {
        public override void Enter(Monster monster)
        {
            base.Enter(monster);
            // Debug.Log("idle enter");
            monster.state = EMonsterState.Idle;

            monster.idleElapsedTime = 0;
            monster.idleEndTime = Random.Range(3, 5); // 대기 시간을 적정범위 내에서 랜덤
            
            if (monster.nav.hasPath)
                monster.nav.ResetPath();
        }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            monster.idleElapsedTime += Time.deltaTime;
            if (monster.idleElapsedTime > monster.idleEndTime)
            {
                monster.fsm.ChangeState(EMonsterState.Patrol);
                return;
            }
            
            if (monster.playerInSight) // 플레이어가 시야에 들어오면
            {
                if (CheckForRunaway(monster))
                    return;
                
                // https://forum.unity.com/threads/getting-the-distance-in-nav-mesh.315846/
                if (monster.playerDist > monster.attackRange) // 타깃이 공격 사정거리보다 멀면
                {
                    monster.fsm.ChangeState(EMonsterState.ChasePlayer);
                    return;
                }
                else // 타깃이 공격 사정거리 안이면
                {
                    // 공격 쿨타임 추가?
                    monster.fsm.ChangeState(EMonsterState.Engage);
                    return;
                }
            }
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
            // Debug.Log("idle exit");
        }

        public bool CheckForRunaway(Monster monster)
        {
            if (monster.monsterType == EMonsterType.General_Ranged)
            {
                if (monster.playerDist < monster.runawayDistance)
                {
                    monster.fsm.ChangeState(EMonsterState.Runaway);
                    return true;
                }   
            }
            return false;
        }
    }
}