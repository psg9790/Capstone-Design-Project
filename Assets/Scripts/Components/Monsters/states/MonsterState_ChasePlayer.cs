using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState_ChasePlayer : MonsterState
{
    private float FOVRadius_base;
    private float FOVAngle_base;
    private float chaseFOVRadius_multi = 2f; 
    private float chaseFOVAngle = 360f;
    
    public MonsterState_ChasePlayer(Monster monster) : base(monster)
    {
    }

    public override void Enter()
    {
        base.Enter();
        monster.state = EMonsterState.ChasePlayer;
        monster.animator.SetBool("ChasePlayer", true);
        FOVRadius_base = monster.fov.viewRadius;
        FOVAngle_base = monster.fov.viewAngle;
        monster.fov.viewRadius *= chaseFOVRadius_multi;
        monster.fov.viewAngle = chaseFOVAngle;
    }

    public override void Execute()
    {
        base.Execute();
        if (monster.playerInSight)
        {
            monster.nav.SetDestination(monster.player.transform.position);
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.animator.SetBool("ChasePlayer", false);
        monster.fov.viewRadius = FOVRadius_base;
        monster.fov.viewAngle = FOVAngle_base;
    }
}
