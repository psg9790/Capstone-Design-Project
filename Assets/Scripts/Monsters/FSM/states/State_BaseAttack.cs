using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.FSM
{
    public class State_BaseAttack : State
    {
       public override void Enter(Monster monster)
        {
            base.Enter(monster);
            monster.state = EMonsterState.BaseAttack;
            monster.animator.SetTrigger("Attack01");
            monster.whileAttack = true;
            if (monster.playerInSight)
            {
                monster.transform.rotation = Quaternion.LookRotation(
                    (monster.player.transform.position - monster.transform.position).normalized, Vector3.up);
            }
        }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            if (!monster.whileAttack) // "공격중" 플래그가 꺼지면 (애니메이션 마지막에 이벤트로 끔)
                monster.fsm.ChangeState(EMonsterState.Idle);
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
        }
    }
}