using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Monsters.FSM
{
    public class State_KnockBack : State
    {
        public override void Enter(Monster monster)
        {
            base.Enter(monster);
            monster.state = EMonsterState.KnockBack;
            monster.nav.enabled = false;
            monster.rigid.isKinematic = false;
            monster.rigid.AddForce(monster.knockback_dir * monster.knockback_power, ForceMode.Impulse);
            monster.whileKnockback = true;
            monster.animator.SetTrigger("Knockback");
        }


        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            if (!monster.whileKnockback) // 애니메이션으로 플래그
            {
                monster.fsm.ChangeState(EMonsterState.Idle);
            }
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
            monster.nav.enabled = true;
            monster.rigid.isKinematic = true;
        }
    }
}