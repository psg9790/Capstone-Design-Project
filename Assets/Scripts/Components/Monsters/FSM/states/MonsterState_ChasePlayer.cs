using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters
{
    public class MonsterState_ChasePlayer : MonsterState
    {
        public MonsterState_ChasePlayer(Monster monster) : base(monster)
        {
        }

        public override void Enter()
        {
            base.Enter();
            monster.state = EMonsterState.ChasePlayer;
            monster.animator.SetBool("Run", true); // 
        }

        public override void Execute()
        {
            base.Execute();
            if (monster.playerInSight) // 추적할 플레이어가 존재하면 움직임
                monster.nav.SetDestination(monster.player.transform.position);
        }

        public override void Exit()
        {
            base.Exit();
            monster.animator.SetBool("Run", false);
            monster.nav.ResetPath();
        }
    }
}