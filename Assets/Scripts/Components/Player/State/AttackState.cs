using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class AttackState : BaseState
    {
        
        public AttackState(PlayerController controller) : base(controller)
        {
            
        }
        public override void OnEnterState()
        {
            UnityEngine.Debug.Log("Attack enter");
            Ray ray = Controller.cam.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            RaycastHit hit;
            Vector3 looking = default;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
                looking = hit.point - Controller.transform.position;
                LookAt(hit.point - Controller.transform.position);
            }
            attack();
            Player.Instance.weaponManager.Weapon?.Attack(this, looking);
        }
        public void attack()
        {
            // isAttackReady = equipWeapon.rate < attackDelay;
        
            // equipWeapon.use();
            Player.Instance.nav.ResetPath();
            
            Player.Instance.animator.SetTrigger("attack");
            // anim.SetTrigger("Bow_attack");

        }

        
        public override void OnUpdateState()
        {
            
        }

        public override void OnFixedUpdateState()
        {
            
        }

        
        public override void OnExitState()
        {
            Player.Instance.animator.ResetTrigger("attack");
            
        }
        protected void LookAt(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                Quaternion targetAngle = Quaternion.LookRotation(direction);
                Controller.transform.rotation = targetAngle;
            }
        }
    }
    
}