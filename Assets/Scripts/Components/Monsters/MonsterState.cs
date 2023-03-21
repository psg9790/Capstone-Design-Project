public class MonsterState
{
    protected Monster monster;

    public MonsterState(Monster monster)
    {
        this.monster = monster;
    }

    public virtual void Enter()
    {
    }

    public virtual void Execute()
    {
    }

    public virtual void Exit()
    {
    }
}


