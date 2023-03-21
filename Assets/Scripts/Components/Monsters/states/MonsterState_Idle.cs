public class MonsterState_Idle : MonsterState
{
    public MonsterState_Idle(Monster monster) : base(monster)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        monster._state = EMonsterState.Idle;
    }

    public override void Execute()
    {
        base.Execute();
        Debug.Log("idle");
    }

    public override void Exit()
    {
        base.Exit();
    }
}
