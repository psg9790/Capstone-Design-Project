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