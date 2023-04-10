namespace Monsters
{
    public class MonsterStateMachine
    {
        private Monster monster; // 이 머신을 이용할 몬스터
        private MonsterState curstate; // 이 머신을 사용할 몬스터의 현재 상태

        public MonsterStateMachine(Monster monster)
        {
            this.monster = monster;
        }

        public void ChangeState(EMonsterState state)
        {
            if (!ReferenceEquals(curstate, null))
            {
                curstate.Exit(monster); // 현재 상태가 존재하면 상태를 종료하는 메서드를 호출해줌
            }

            curstate = MonsterStateList.Instance.FindState(state); // 상태를 갈이끼워줌
            curstate.Enter(monster); // 새로운 상태로 진입하는 함수를 호출해줌
        }

        public void Execute()
        {
            if (!ReferenceEquals(curstate, null))
            {
                curstate.Execute(monster); // 현재 상태의 Update
            }
        }
    }
}