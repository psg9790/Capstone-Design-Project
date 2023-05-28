using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.FSM
{
    public class State_ChasePlayer : State
    {
        public override void Enter(Monster monster)
        {
            base.Enter(monster);
            monster.state = EMonsterState.ChasePlayer;
            monster.animator.SetBool("Run", true);
            // if (monster.sound.ContainsKey(EMonsterState.ChasePlayer))
            // {
            //     // 음성 재생
            // }
        }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            
            if (monster.playerInSight)
            {
                monster.nav.SetDestination(monster.player.transform.position);

                if (monster.playerDist < monster.attackRange) // 플레이어가 공격 사정거리 안에 들어왔을 때
                {
                    monster.fsm.ChangeState(EMonsterState.Engage);
                }
            }
            else // 플레이어를 시야에서 놓쳤을 시
            {
                monster.fsm.ChangeState(EMonsterState.Idle);
            }
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
            monster.animator.SetBool("Run", false);
            monster.nav.ResetPath();
        }
    }
}