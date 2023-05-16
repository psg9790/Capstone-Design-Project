using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class MoveState : BaseState
    {
        public static bool IsMove = false;
        protected Vector3 moveto;
        private Vector3 moveTarget;
        public MoveState(PlayerController controller) : base(controller)
        {
            
        }
        public override void OnEnterState()
        {
            IsMove = true;
            // UnityEngine.Debug.Log("MoveState enter");
            Ray ray = CameraController.Instance.cam.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
                Player.Instance.nav.SetDestination(hit.point);
                moveto = hit.point;
            }
            Player.Instance.animator.SetBool("speed", true);
        }

        public override void OnUpdateState()
        {
            NavRotation();
            if (InputManager.Instance.GetAction(InputKey.RightClick).IsPressed())
            {
                OnEnterState();
            }
            moveTarget = moveto;
            // UnityEngine.Debug.Log("MoveState");
            Vector3 dist = moveTarget - Player.Instance.transform.position;
            if (IsMove && dist.magnitude <= 0.1f)
            {
                IsMove = false;
                Player.Instance.stateMachine.ChangeState(StateName.Idle);
            }
            
            
        }
        void NavRotation()
        {
            if (!Player.Instance.nav.hasPath)
                return;
        
            Vector2 forward = new Vector2(Player.Instance.transform.position.z, Player.Instance.transform.position.x);
            Vector2 steeringTarget = new Vector2(Player.Instance.nav.steeringTarget.z, Player.Instance.nav.steeringTarget.x);
    
            //방향을 구한 뒤, 역함수로 각을 구한다.
            Vector2 dir = steeringTarget - forward;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    
            //방향 적용
            Player.Instance.transform.eulerAngles = Vector3.up * angle;
        }

        public override void OnFixedUpdateState()
        {
            
        }

        
        public override void OnExitState()
        {
            // UnityEngine.Debug.Log("MoveState out");
            Player.Instance.animator.SetBool("speed", false);
        }
    }
}