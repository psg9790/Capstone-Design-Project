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
        monster.state = EMonsterState.Patrol;
        NavMeshPath path = new NavMeshPath();
        while (true)
        {
            /*
             몬스터가 스폰포인트에서 랜덤 좌표를 가져오는데
             그 좌표가 navigation을 사용하는데 적절하지 않은 좌표일 수 있어서 (Not Walkable)
             적절한 좌표를 가져올 때까지 경로 탐색을 반복함
             */
            monster.patrolPoint = monster.spawner.GetRandomPosInPatrolRadius();
            if (monster.nav.CalculatePath(monster.patrolPoint, path))
            {
                monster.nav.SetDestination(monster.patrolPoint);
                break;
            }
        }
        monster.animator.SetBool("Patrol", true);   // 순찰 애니메이션
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
    }

    public override void Exit()
    {
        base.Exit();
        monster.animator.SetBool("Patrol", false);
        monster.nav.ResetPath();
    }
}