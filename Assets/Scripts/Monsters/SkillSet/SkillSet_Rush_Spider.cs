using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.Skill
{
    public class SkillSet_Rush_Spider : SkillSet
    {
        void Update()
        {
        }

        public override void Terminate()
        {
        }

        public override void DoPossibleEngage()
        {
            monster.animator.SetTrigger("Skill00"); // 사용 가능한 스킬이 없으면 그냥 평타
        }
    }
}