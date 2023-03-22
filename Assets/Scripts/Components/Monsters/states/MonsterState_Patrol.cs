using UnityEngine;

public class MonsterState_Patrol : MonsterState
{
    public MonsterState_Patrol(Monster monster) : base(monster)
    {
    }

    public override void Enter()
    {
        base.Enter();
        monster._state = EMonsterState.Patrol;
        monster.patrolPoint = monster.spawner.GetRandomPosInPatrolRadius();
        monster.nav.SetDestination(monster.patrolPoint);
        monster.animator.SetBool("Patrol", true);
    }

    public override void Execute()
    {
        base.Execute();
        Vector3 diff = monster.transform.position - monster.patrolPoint;
        if (diff.magnitude < 0.1f)
        {
            monster.fsm.ChangeState(new MonsterState_Idle(monster));
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.animator.SetBool("Patrol", false);
        monster.nav.ResetPath();
    }
}