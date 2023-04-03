using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState_Runaway : MonsterState
{
    public MonsterState_Runaway(Monster monster) : base(monster)
    {
    }

    public override void Enter()
    {
        base.Enter();
        monster.state = EMonsterState.Runaway;
        monster.animator.SetBool("Run", true);  // 
    }

    public override void Execute()
    {
        base.Execute();
        if (monster.playerInSight)
        {
            monster.nav.SetDestination(monster.transform.position + (monster.transform.position - monster.player.transform.position).normalized);
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.animator.SetBool("Run", false);  // 
        monster.nav.ResetPath();
    }
}
