using UnityEngine;
using Random = UnityEngine.Random;

namespace Monsters
{
    public class MonsterState_Idle : MonsterState
    {
        private float elapsedTime;
        private float endTime;

        public MonsterState_Idle(Monster monster) : base(monster)
        {
        }

        public override void Enter()
        {
            base.Enter();
            monster.state = EMonsterState.Idle;

            endTime = Random.Range(3, 5);
            elapsedTime = 0;

            // monster.idleElapsedTime = 0f;
            // monster.idleToPatrolTime = Random.Range(3, 5);  // 대기 시간을 적정범위 내에서 랜덤
            if (monster.nav.hasPath)
                monster.nav.ResetPath();
        }

        public override void Execute()
        {
            base.Execute();
            elapsedTime += Time.deltaTime;
            if (elapsedTime > endTime)
            {
                monster.fsm.ChangeState(new MonsterState_Patrol(monster));
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}