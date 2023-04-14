using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.Skill
{
    public class SkillSet_General_Ranged : SkillSet
    {
        public GameObject basicMagic;

        private void GenerateBaseMagic()
        {
            BasicMagic magic = Instantiate(basicMagic).GetComponent<BasicMagic>();
            magic.GetComponent<HitBox>().SetDamage(new Damage(monster.heart.ATK, CC_type.None));
            magic.Init(this.transform.position + transform.forward * 0.25f + Vector3.up,
                transform.forward, 2.5f);
        }

        public override void Terminate()
        {
            basicMagic.GetComponent<HitBox>().COLLIDER_OFF();
        }

        public override void DoPossibleEngage()
        {
            
        }
    }
}