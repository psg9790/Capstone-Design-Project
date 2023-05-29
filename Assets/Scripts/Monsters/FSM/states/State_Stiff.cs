using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.FSM
{
    public class State_Stiff : State
    {
        public override void Enter(Monster monster)
        {
            base.Enter(monster);
            monster.state = EMonsterState.Stiff;
            monster.animator.SetTrigger("Stiff");
            monster.whileStiff = true;
        }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            if (!monster.whileStiff)
            {
                monster.fsm.ChangeState(EMonsterState.Idle);
                return;
            }
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
            monster.whileStiff = false;
        }
    }
}