namespace Monsters
{
    public class MonsterState
    {
        protected Monster monster;

        public MonsterState(Monster monster)
        {
            this.monster = monster;
        }

        public virtual void Enter() // 몬스터가 이 상태로 진입할 때 실행할 것
        {
        }

        public virtual void Execute() // 몬스터가 현재 상태에서 실행할 것들
        {
        }

        public virtual void Exit() // 몬스터가 현재 상태에서 나갈 때 실행할 것
        {
        }
    }
}