using UnityEngine;
using UnityEngine.AI;

public class MonsterState_Patrol : MonsterState
{
    public MonsterState_Patrol(Monster monster) : base(monster)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        monster._state = EMonsterState.Patrol;
        NavMeshPath path = new NavMeshPath();
        while (true)
        {
            monster.patrolPoint = monster.spawner.GetRandomPosInPatrolRadius();
            if (monster.nav.CalculatePath(monster.patrolPoint, path))
            {
                monster.nav.SetDestination(monster.patrolPoint);
                break;
            }
        }
        
        monster.animator.SetBool("Patrol", true);
    }

    public override void Execute()
    {
        base.Execute();
        // https://answers.unity.com/questions/324589/how-can-i-tell-when-a-navmesh-has-reached-its-dest.html
        if (monster.nav.remainingDistance < monster.nav.stoppingDistance)
        {
            if (!monster.nav.hasPath || monster.nav.velocity.sqrMagnitude == 0f)
            {
                monster.fsm.ChangeState(new MonsterState_Idle(monster));
            }
        }
        // Debug.DrawRay(monster.patrolPoint, Vector3.up * 3, Color.blue);
    }

    public override void Exit()
    {
        base.Exit();
        monster.animator.SetBool("Patrol", false);
        monster.nav.ResetPath();
    }
}