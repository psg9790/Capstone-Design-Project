using UnityEngine;
using Random = UnityEngine.Random;

namespace Monsters
{
    public class MonsterState_Idle : MonsterState
    {
        public override void Enter(Monster monster)
        {
            base.Enter(monster);
            monster.state = EMonsterState.Idle;

            monster.idleElapsedTime = 0;
            monster.idleEndTime = Random.Range(3, 5);
            // endTime = Random.Range(3, 5);
            // elapsedTime = 0;

            // monster.idleElapsedTime = 0f;
            // monster.idleToPatrolTime = Random.Range(3, 5);  // 대기 시간을 적정범위 내에서 랜덤
            if (monster.nav.hasPath)
                monster.nav.ResetPath();
        }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            monster.idleElapsedTime += Time.deltaTime;
            if (monster.idleElapsedTime > monster.idleEndTime)
            {
                monster.fsm.ChangeState(EMonsterState.Patrol);
                return;
            }
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
        }
    }
}