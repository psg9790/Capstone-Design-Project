using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class DashState : BaseState
    {
        Queue<Vector3> inputDirectionBuffer = new Queue<Vector3>();
        public static bool IsDash = false;
        private float dashStartTime;
        private Vector3 dashDirection;
        
        public float dashDistance; // 대쉬 거리
        public float dashDuration; // 대쉬 시간
        public float dashCooldown; // 대쉬 쿨다운
        // public bool IsDash { get; set; }
        
        public DashState(PlayerController controller) : base(controller)
        {
            
        }
        public override void OnEnterState()
        {
            UnityEngine.Debug.Log("Dash enter");
            dashDistance = Controller.dashDistance;
            dashDuration = Controller.dashDuration;
            dashCooldown = Controller.dashCooldown;
            if (!IsDash)
            {
                Ray ray = Controller.cam.ScreenPointToRay(InputManager.Instance.GetMousePosition());
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
                {
                    Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
                    LookAt(hit.point - Controller.transform.position);
                }

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 clickPosition = hit.point;

                    // 현재 위치와 대쉬할 방향 벡터 계산
                    Vector3 currentPosition = Controller.transform.position;
                    Vector3 dashVector = clickPosition - currentPosition;
                    dashVector.y = 0f;
                    // float _dashDistance = dashVector.magnitude;
                
                    // 대쉬할 방향 벡터 저장
                    dashDirection = dashVector.normalized;
                
                    // NavMeshAgent 비활성화
                    Player.Instance.nav.enabled = false;
                
                    // 대쉬 시작
                    IsDash = true;
                    dashStartTime = Time.time;
                    Player.Instance.animator.SetTrigger("doDodge");
                }
            }
            else
            {
                Player.Instance.stateMachine.ChangeState(StateName.Idle);
            }
        }
        
        protected void LookAt(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                Quaternion targetAngle = Quaternion.LookRotation(direction);
                Controller.transform.rotation = targetAngle;
            }
        }
        public override void OnUpdateState()
        {
            
            // 일정 시간이 지난 후에 NavMeshAgent 활성화
            if (Time.time >= dashStartTime + dashDuration)
            {
                
                Player.Instance.stateMachine.ChangeState(StateName.Idle);
            }
            else
            {
                // 플레이어 이동
                Vector3 dashVelocity = dashDirection * (dashDistance / dashDuration);
                Controller.transform.position += dashVelocity * Time.deltaTime;
            }
        }

        public override void OnFixedUpdateState()
        {
            
        }

        public override void OnExitState()
        {
            UnityEngine.Debug.Log("Dash out");
            IsDash = false;
            Player.Instance.nav.enabled = true;
            Controller.isDashing = false;

        }
        
    }
}