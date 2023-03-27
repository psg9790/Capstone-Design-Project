
public class MonsterStateMachine
{
    private Monster monster;
    private MonsterState curstate;
    
    public MonsterStateMachine(Monster monster)
    {
        this.monster = monster;
    }

    public void ChangeState(MonsterState nextstate)
    {
        if (!ReferenceEquals(curstate, null))
        {
            curstate.Exit();
        }
        curstate = nextstate;
        curstate.Enter();
    }

    public void Execute()
    {
        if (!ReferenceEquals(curstate, null))
        {
            curstate.Execute();
        }
    }
}
