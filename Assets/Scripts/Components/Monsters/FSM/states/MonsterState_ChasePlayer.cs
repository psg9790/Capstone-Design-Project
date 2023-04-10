using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters
{
    public class MonsterState_ChasePlayer : MonsterState
    {
        public override void Enter(Monster monster)
        {
            base.Enter(monster);
            monster.state = EMonsterState.ChasePlayer;
            monster.animator.SetBool("Run", true); // 
        }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            if (monster.playerInSight) // 추적할 플레이어가 존재하면 움직임
                monster.nav.SetDestination(monster.player.transform.position);
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
            monster.animator.SetBool("Run", false);
            monster.nav.ResetPath();
        }
    }
}