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
                Ray ray = Controller.cam.ScreenPointToRay(InputManager.Instance.GetMousePosition());
                RaycastHit hit;
                Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
                
                // 수정할 부분
                // 여기를 땅찍는쪽으로 방향 받는게 아니라 아무대나 눌러도 방향만 받아오게 한 다음에
                // 업데이트에서 이동할 위치에 위에서 레이져 쏴서 그 곳이 워커블인지 확인하면
                // 맵밖 찍어도 구르기를 하고 알아서 충돌처리나 기울기 처리도 될거같음
                
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
                {
                    Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
                    Vector3 pltp = Player.Instance.transform.position;
                    LookAt(hit.point - pltp);
                    
                    dashDirection = (hit.point - pltp).normalized;
                    
                    // NavMeshAgent 비활성화
                    Player.Instance.nav.enabled = false;
                
                    // 대쉬 시작
                    IsDash = true;
                    dashStartTime = Time.time;
                    Player.Instance.animator.SetTrigger("doDodge");
                }

                
                
                // if (Physics.Raycast(ray, out hit))
                // {
                //     Vector3 clickPosition = hit.point;
                //
                //     // 현재 위치와 대쉬할 방향 벡터 계산
                //     Vector3 currentPosition = Controller.transform.position;
                //     Vector3 dashVector = clickPosition - currentPosition;
                //     dashVector.y = 0f;
                //     // float _dashDistance = dashVector.magnitude;
                //
                //     // 대쉬할 방향 벡터 저장
                //     // dashDirection = dashVector.normalized;
                //     
                // }
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
                RaycastHit hit;
                Vector3 dashVelocity = dashDirection * (dashDistance / dashDuration);//얼마나 이동할지 계산
                Vector3 pp = Player.Instance.transform.position;
                Vector3 nextpos = pp + (5 * Vector3.forward);
                nextpos += Vector3.up;
                Ray ray = new Ray(nextpos,Vector3.down);
                Debug.DrawRay(nextpos, Vector3.down, Color.red, 1f);
                if (Physics.Raycast(ray, out hit,2f, 1 << LayerMask.NameToLayer("Walkable")))
                {
                    
                    Player.Instance.transform.position += dashVelocity * Time.deltaTime;
                }
                // Debug.DrawRay(pp,dashDirection, Color.red,1f);//플레이어 앞에 0.5f만큼 레이져
                // 만약 플레이어 앞에 가로막는 벽이 없다면 이동 아니면 이동x
                // if (!Physics.Raycast(pp, dashDirection, 1f, 1 << LayerMask.NameToLayer("WALL")))
                // {
                //     Player.Instance.transform.position += dashVelocity * Time.deltaTime;
                // }
                

                // Player.Instance.rigidbody.AddForce(dashVelocity * Time.deltaTime);
            }
        }

        public override void OnFixedUpdateState()
        {
            
        }

        public override void OnExitState()
        {
            // UnityEngine.Debug.Log("Dash out");
            IsDash = false;
            Player.Instance.nav.enabled = true;
            Controller.isDashing = false;

        }
        
    }
}