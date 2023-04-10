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
        if (monster.playerInSight)
        {
            monster.transform.rotation = Quaternion.LookRotation(
                (monster.player.transform.position - monster.transform.position).normalized, Vector3.up);
        }
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
