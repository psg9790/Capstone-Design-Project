using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters
{
    public class MonsterState_Runaway : MonsterState
    {
        public override void Enter(Monster monster)
        {
            base.Enter(monster);
            monster.state = EMonsterState.Runaway;
            monster.animator.SetBool("Run", true); // 
        }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            if (monster.playerInSight)
            {
                if (monster.playerDist > monster.runawayDistance)
                {
                    monster.fsm.ChangeState(EMonsterState.Idle);
                    return;
                }
                monster.nav.SetDestination(monster.transform.position +
                                           (monster.transform.position - monster.player.transform.position).normalized);
            }
            else
            {
                monster.fsm.ChangeState(EMonsterState.Idle);
                return;
            }
            // 탈출 만들어
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
            monster.animator.SetBool("Run", false); // 
            monster.nav.ResetPath();
        }
    }
}