using Random = UnityEngine.Random;

public class MonsterState_Idle : MonsterState
{
    public MonsterState_Idle(Monster monster) : base(monster)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        monster.state = EMonsterState.Idle;
        monster.idleElapsedTime = 0f;
        monster.idleToPatrolTime = Random.Range(3, 5);  // 대기 시간을 적정범위 내에서 랜덤
        if(monster.nav.hasPath)
            monster.nav.ResetPath();
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
