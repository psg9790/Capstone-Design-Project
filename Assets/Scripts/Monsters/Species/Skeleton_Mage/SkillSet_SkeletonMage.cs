using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.Skill
{
    public class SkillSet_SkeletonMage : SkillSet
    {
        [SerializeField] private Transform bulletPos;

        public GameObject baseAttackHitbox;


        private Coroutine lerpBaseAttackCo;

        private IEnumerator lerpIE()
        {
            while (true)
            {
                Vector3 dir = Player.Instance.transform.position - transform.position;
                dir.y = 0;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 0.025f);
                yield return null;
            }
        }
        public void LerpBeforeBaseAttack()
        {
            lerpBaseAttackCo = StartCoroutine(lerpIE());
        }
        public void BaseAttack()
        {
            if (lerpBaseAttackCo != null)
            {
                StopCoroutine(lerpBaseAttackCo);
            }
            if (baseAttackHitbox != null)
            {
                HitBox bullet = Instantiate(baseAttackHitbox).GetComponent<HitBox>();
                bullet.BulletParticle_Play(heart, bulletPos.position, transform.forward);
            }
        }

        public override void SetMonsterStatByLevel(short level)
        {
        }

        public override void DoPossibleEngage()
        {
            monster.animator.SetTrigger("BaseAttack");
        }
    }
}