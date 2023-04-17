namespace Monsters.FSM
{
    public class State
    {
        public virtual void Enter(Monster monster) // 몬스터가 이 상태로 진입할 때 실행할 것
        {
        }

        public virtual void Execute(Monster monster) // 몬스터가 현재 상태에서 실행할 것들
        {
        }

        public virtual void Exit(Monster monster) // 몬스터가 현재 상태에서 나갈 때 실행할 것
        {
        }
    }
}