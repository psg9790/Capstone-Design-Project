using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState_BaseAttack : MonsterState
{
    public MonsterState_BaseAttack(Monster monster) : base(monster)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        monster.state = EMonsterState.BaseAttack;
        monster.animator.SetTrigger("Attack01");
        monster.whileAttack = true;
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        monster.ExtendSight();
    }
}
