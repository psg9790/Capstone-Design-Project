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
