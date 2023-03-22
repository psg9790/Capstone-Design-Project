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
    }

    public override void Execute()
    {
        base.Execute();
        Debug.Log("chasing player");
    }

    public override void Exit()
    {
        base.Exit();
    }
}
