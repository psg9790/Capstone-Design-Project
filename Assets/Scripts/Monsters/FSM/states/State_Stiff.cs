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
            monster.stiffElapsed = 0;
            // monster.transform.rotation = Quaternion.LookRotation(monster.gotAttackDir);
            monster.animator.SetTrigger("Stiff");
            UnityEngine.Debug.Log("stiff");
        }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            monster.stiffElapsed += Time.deltaTime;
            if (monster.stiffElapsed > monster.stiffTime)
            {
                monster.fsm.ChangeState(EMonsterState.Idle);
                return;
            }
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
            UnityEngine.Debug.Log("stiff end. elapsed: " + monster.stiffElapsed);
        }
    }
}