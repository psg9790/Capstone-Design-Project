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
            // UnityEngine.Debug.Log("Dash enter");
            dashDistance = Player.Instance.dashDistance;
            dashDuration = Player.Instance.dashDuration;
            if (!IsDash)
            {
                Ray ray = CameraController.Instance.cam.ScreenPointToRay(InputManager.Instance.GetMousePosition());
                RaycastHit hit;
                int mask = (1 << LayerMask.NameToLayer("Walkable")) | (1 << LayerMask.NameToLayer("Click"));
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                {
                    Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
                    Vector3 pltp = Player.Instance.transform.position;
                    LookAt(hit.point - pltp);
                    
                    dashDirection = (hit.point - pltp).normalized;
                    
                    // NavMeshAgent 비활성화
                    Player.Instance.nav.enabled = false;
                
                    // 대쉬 시작
                    Player.Instance.heart.immune = true;
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
                
                RaycastHit hit;
                Vector3 dashVelocity = Vector3.zero;
                Vector3 pp = Player.Instance.transform.position;
                Vector3 nextpos = pp + Player.Instance.transform.forward + Vector3.up; // 플레이어 바로 앞 살짝 위
                Ray ray = new Ray(nextpos,Vector3.down);
                Debug.DrawRay(nextpos, Vector3.down, Color.red, 5f);
                // 다음 예상 위치에서 바닥까지 레이져를 쏴서 다음 위치 벡터 찾기 
                int mask = (1 << LayerMask.NameToLayer("Walkable")) | (1 << LayerMask.NameToLayer("Click"));
                if (Physics.Raycast(ray, out hit,Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
                {
                    // 만약 플레이어 앞에 Wall 이면 이동x
                    Debug.DrawRay(pp,dashDirection, Color.red,1f);//플레이어 앞에 레이져 발사
                    if (Physics.Raycast(pp, dashDirection, 1f, 1 << LayerMask.NameToLayer("WALL")))
                    {
                        // Debug.Log("cant dash");
                        return;
                    }
                    //새로 찍은 이동할 방향벡터로 플레이어 이동시키기
                    dashDirection = hit.point - pp;
                    dashVelocity = dashDirection * (dashDistance / dashDuration);
                    Player.Instance.transform.position += dashVelocity * Time.deltaTime;
                }
            }
        }

        public override void OnFixedUpdateState()
        {
            
        }

        public override void OnExitState()
        {
            // UnityEngine.Debug.Log("Dash out");
            IsDash = false;
            Player.Instance.heart.immune = false;
            Player.Instance.nav.enabled = true;
            Controller.isDashing = false;

        }
        
    }
}