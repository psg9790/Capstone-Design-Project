using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.FSM
{
    public class State_Die : State
    {
        public override void Enter(Monster monster)
        {
            base.Enter(monster);
            monster.transform.rotation = Quaternion.LookRotation(monster.gotAttackDir);
            monster.bodyCollider.enabled = false;
            monster.animator.SetTrigger("Die");
            monster.afterDeadElapsed = 0f;
            Debug.Log("enter die state");
            // 아이템 생성
        }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            // 모션 끝나면 Exit
            if (monster.animator.GetCurrentAnimatorStateInfo(0).IsName("Die")
                && monster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                monster.afterDeadElapsed += Time.deltaTime;
                if (monster.afterDeadElapsed >= monster.afterDeadTime)
                {
                    Exit(monster);
                }
            }
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
            monster.Eliminate();
        }
    }
}