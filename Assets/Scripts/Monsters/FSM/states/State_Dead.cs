using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.FSM
{
    public class State_Dead : State
    {
        public override void Enter(Monster monster)
        {
            base.Enter(monster);
            monster.bodyCollider.enabled = false;
            // 아이템 생성
        }

        public override void Execute(Monster monster)
        {
            base.Execute(monster);
            // 모션 끝나면 Exit
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
            monster.Eliminate();
        }
    }
}