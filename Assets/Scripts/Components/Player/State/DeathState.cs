using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class DeathState : BaseState
    {
        public DeathState(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            Controller.isDeath = true;
            Debug.Log("YOU DIE");
            Player.Instance.animator.SetBool("Death", true);
            Player.Instance.animator.SetTrigger("onDeath");

            if (GrowthLevelManager.Instance != null) // 성장형 던전 사망
            {
                // GameManager.Instance.EndOfGrowthDungeon(GrowthLevelManager.Instance.worldLevel); // 주사위 + 최대 층수 저장
                GameManager.Instance.Death_GrowthUI();
            }
        }

        public override void OnUpdateState()
        {
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
            Player.Instance.animator.SetBool("Death", false);
            Controller.isDeath = false;
        }
    }
}