// 몬스터 프리팹에 장착함으로써 몬스터는 이 컴포넌트의 스킬을 실행하기만 하면 됨
// 이렇게 해놓은 이유는, 이 클래스를 상속받아서 여러가지 형태의 스킬셋을 구성할 수 있을것 같아서임
// 몬스터마다 개성있는 스킬셋을 만들 수 있도록 하는게 이 스크립트의 존재 의의.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.Skill
{
    public abstract class SkillSet : MonoBehaviour
    {
        protected Monster monster;
        protected Heart heart;

        private void Awake()
        {
            monster = GetComponent<Monster>();
            heart = GetComponent<Heart>();
        }
        protected void EndEngage()
        {
            monster.whileEngage = false;
        }

        public virtual void Terminate()
        {
            EndEngage();
        } // cc기나 캔슬에 의해 현재 공격중인 오브젝트를 모두 초기화하고 꺼줄 필요가 있음

        public abstract void DoPossibleEngage();
    }
}