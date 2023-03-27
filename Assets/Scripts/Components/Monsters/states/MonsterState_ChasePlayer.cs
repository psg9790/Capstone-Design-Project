using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState_ChasePlayer : MonsterState
{
    public MonsterState_ChasePlayer(Monster monster) : base(monster)
    {
    }

    public override void Enter()
    {
        base.Enter();
        monster.state = EMonsterState.ChasePlayer;
        monster.animator.SetBool("ChasePlayer", true);  // 
    }

    public override void Execute()
    {
        base.Execute();
        monster.ExtendSight();  // 추적할 때 시야증가
        if (monster.playerInSight)  // 추적할 플레이어가 존재하면 움직임
            monster.nav.SetDestination(monster.player.transform.position);
    }

    public override void Exit()
    {
        base.Exit();
        monster.animator.SetBool("ChasePlayer", false);
        monster.nav.ResetPath();
    }
}
