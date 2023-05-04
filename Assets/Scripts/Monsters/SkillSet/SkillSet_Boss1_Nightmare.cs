using System.Collections;
using System.Collections.Generic;
using Monsters.FSM;
using UnityEngine;

namespace Monsters.Skill
{
    public class SkillSet_Boss1_Nightmare : SkillSet
    {
        public override void DoPossibleEngage()
        {
            // throw new System.NotImplementedException();
            Debug.Log("attack");
            monster.animator.SetTrigger("Skill00"); // 사용 가능한 스킬이 없으면 그냥 평타
        }
    }
}