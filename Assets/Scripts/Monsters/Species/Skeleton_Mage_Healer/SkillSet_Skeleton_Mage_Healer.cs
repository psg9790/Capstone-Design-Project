using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monsters.Skill
{
    public class SkillSet_Skeleton_Mage_Healer : SkillSet
    {
        public HitBox healEffect;
        void BaseAttack()
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, 10f, 1 << LayerMask.NameToLayer("Monster"));
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].TryGetComponent<Heart>(out Heart target))
                {
                    target.Restore_CUR_HP(heart.ATK);
                    HitBox healVFX = Instantiate(healEffect, target.transform);
                    // healVFX.transform.position = target.transform.position;
                    healVFX.Particle_Play(target);
                    // Debug.Log(target.gameObject.name);
                }
            }
        }
        public override void DoPossibleEngage()
        {
            monster.animator.SetTrigger("BaseAttack");
        }
        
        // 스킬셋에 맞는 몬스터의 고유값을 공유하기 위해서 재정의를 사용했음
        private static List<float> atk_byLevel = new List<float>();
        private static List<float> hp_byLevel = new List<float>();
        private static List<float> def_byLevel = new List<float>();
        private static List<float> atkspeed_byLevel = new List<float>();
        private static List<float> movementspeed_byLevel = new List<float>();

        public override void SetMonsterStatByLevel(short level)
        {
            if (atk_byLevel.Count == 0) // 새로운 전역 레벨 변수 추가
            {
                float calcatk, calchp, calcdef, calcatkspeed, calcmovespeed;
                atk_byLevel.Add(calcatk = heart.ATK);
                hp_byLevel.Add(calchp = heart.MAX_HP);
                def_byLevel.Add(calcdef = heart.DEF);
                atkspeed_byLevel.Add(calcatkspeed = heart.ATK_SPEED);
                movementspeed_byLevel.Add(calcmovespeed = heart.MOVEMENT_SPEED);
                // for (int i = 0; i < GrowthLevelManager.Instance.maxLevel; i++)
                // {
                //     atk_byLevel.Add(calcatk *= statGrowthByLevelUp);
                //     hp_byLevel.Add(calchp *= statGrowthByLevelUp);
                //     def_byLevel.Add(calcdef *= statGrowthByLevelUp);
                //     atkspeed_byLevel.Add(calcatkspeed += (statGrowthByLevelUp * 0.05f));
                //     movementspeed_byLevel.Add(calcmovespeed += (statGrowthByLevelUp * 0.05f));
                // }
            }

            if (atk_byLevel.Count <= level)
            {
                for (int i = atk_byLevel.Count - 1; i < level; i++)
                {
                    atk_byLevel.Add(atk_byLevel[i] *= statGrowthByLevelUp);
                    hp_byLevel.Add(hp_byLevel[i] *= statGrowthByLevelUp);
                    def_byLevel.Add(def_byLevel[i] *= statGrowthByLevelUp);
                    atkspeed_byLevel.Add(atkspeed_byLevel[i] += (statGrowthByLevelUp * 0.05f));
                    movementspeed_byLevel.Add(movementspeed_byLevel[i] += (statGrowthByLevelUp * 0.05f));
                }
            }

            heart.SetStat(atk_byLevel[level], hp_byLevel[level], def_byLevel[level],
                atkspeed_byLevel[level], movementspeed_byLevel[level]);
        }
    }
}