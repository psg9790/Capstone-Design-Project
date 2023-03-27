using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState_IdleBattle : MonsterState
{
    public MonsterState_IdleBattle(Monster monster) : base(monster)
    {
    }

    public override void Enter()
    {
        base.Enter();
        monster.state = EMonsterState.IdleBattle;
        monster.animator.SetBool("IdleBattle" , true);
        monster.ExtendSight();
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        // monster.animator.SetBool("IdleBattle" , false);
    }
}
