using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CharacterController
{
    public class SkillState : BaseState
    {
        
        public SkillState(PlayerController controller) : base(controller)
        {
            
        }
        public override void OnEnterState()
        {
            UnityEngine.Debug.Log("Skill enter");
            Ray ray = Controller.cam.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            RaycastHit hit;
            Vector3 looking = default;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
                looking = hit.point - Controller.transform.position;
                LookAt(hit.point - Controller.transform.position);
            }
            Player.Instance.nav.ResetPath();
            
            Player.Instance.animator.SetTrigger("skill");
            Player.Instance.weaponManager.Weapon?.Skill();
        }
        

        
        public override void OnUpdateState()
        {
            
        }

        public override void OnFixedUpdateState()
        {
            
        }

        
        public override void OnExitState()
        {
            
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