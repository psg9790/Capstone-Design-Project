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
            Debug.Log("knockback " + monster.knockback_power + monster.knockback_dir);
            monster.nav.enabled = false;
            monster.rigid.isKinematic = false;
            monster.rigid.AddForce(monster.knockback_dir * monster.knockback_power, ForceMode.Impulse);
            monster.whileKnockback = true;
            monster.animator.SetTrigger("Knockback");

            // DOVirtual.DelayedCall(Time.deltaTime, () => AddForce(monster));
        }

        // private bool forced;
        // private void AddForce(Monster monster)
        // {
        //     monster.rigid.AddForce(monster.knockback_dir * monster.knockback_power, ForceMode.Impulse);
        //     forced = true;
        // }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            // 애니메이션으로 플래그 수정할것
            if (!monster.whileKnockback)
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