using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;

namespace Monsters.FSM
{
    public class State_Engage : State
    {
        public override void Enter(Monster monster)
        {
            base.Enter(monster);
            // 가능한 공격 check -> whileAttack = true
            monster.nav.enabled = false;
            monster.rigid.isKinematic = false;
            monster.state = EMonsterState.Engage;
            monster.whileEngage = true;
            monster.DoPossibleEngage();
            monster.transform.rotation =
                Quaternion.LookRotation(monster.player.transform.position - monster.transform.position, Vector3.up);
        }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            // if whileAttack = false, change state to idle
            if (!monster.whileEngage)
            {
                monster.fsm.ChangeState(EMonsterState.Idle);
            }
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
            monster.skillset.Terminate();
            monster.whileEngage = false;
            monster.rigid.isKinematic = true;
            monster.nav.enabled = true;
            // Debug.Log("engage interrupt");
        }
    }
}