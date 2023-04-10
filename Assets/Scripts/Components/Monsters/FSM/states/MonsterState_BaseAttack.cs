using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters
{
    public class MonsterState_BaseAttack : MonsterState
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
        }

        public override void Exit(Monster monster)
        {
            base.Exit(monster);
        }
    }
}