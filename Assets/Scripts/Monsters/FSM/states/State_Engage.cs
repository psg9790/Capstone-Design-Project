using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.FSM
{
    public class State_Engage : State
    {
        public override void Enter(Monster monster)
        {
            base.Enter(monster);
            // 가능한 공격 check -> whileAttack = true
            monster.state = EMonsterState.Engage;
            monster.engage = true;
            monster.DoPossibleEngage();
        }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            // if whileAttack = false, change state to idle
            if (!monster.engage)
            {
                monster.fsm.ChangeState(EMonsterState.Idle);
            }
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
            monster.skillset.Terminate();
        }
    }
}