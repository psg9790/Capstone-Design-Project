using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEditor.Rendering;

namespace CharacterController
{
    
    public enum StateName
    {
        Idle,
        move,
        dash,
        attack,
    }
    public class StateMachine
    {
        public BaseState CurrentState { get; private set; }
        private Dictionary<StateName, BaseState> states = new Dictionary<StateName, BaseState>();

        public StateMachine(StateName stateName, BaseState state)
        {
            AddState(stateName, state);
            CurrentState = GetState(stateName);
        }

        public void AddState(StateName stateName, BaseState state)
        {
            if (!states.ContainsKey(stateName))
            {
                states.Add(stateName, state);
            }
        }

        public BaseState GetState(StateName stateName)
        {
            if (states.TryGetValue(stateName, out BaseState state))
            {
                return state;
            }

            return null;
        }

        public void DeleteState(StateName removeStateName)
        {
            if (states.ContainsKey(removeStateName))
            {
                states.Remove(removeStateName);
            }
        }

        public void ChangeState(StateName nextStateName)
        {
            CurrentState?.OnExitState();
            if (states.TryGetValue(nextStateName, out BaseState newState))
            {
                CurrentState = newState;
            }
            CurrentState?.OnEnterState();
        }

        public void UpdateState()
        {
            CurrentState?.OnUpdateState();
        }

        public void FixedUpdateState()
        {
            CurrentState?.OnFixedUpdateState();
        }
        
    }
    
    public abstract class BaseState
    {
        protected PlayerController Controller { get; private set; }

        public BaseState(PlayerController controller)
        {
            this.Controller = controller;
        }

        public abstract void OnEnterState();
        public abstract void OnUpdateState();
        public abstract void OnFixedUpdateState();
        public abstract void OnExitState();
    }

    public class IdleState : BaseState
    {
        public IdleState(PlayerController controller) : base(controller)
        {
            
        }
        public override void OnEnterState()
        {
            UnityEngine.Debug.Log("Idle enter");
        }

        public override void OnUpdateState()
        {
            UnityEngine.Debug.Log("Idle");
        }

        public override void OnFixedUpdateState()
        {
            
        }

        public override void OnExitState()
        {
            
        }
    }
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
            UnityEngine.Debug.Log("MoveState enter");
            Ray ray = Controller.cam.ScreenPointToRay(InputManager.Instance.GetMousePosition());
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
            UnityEngine.Debug.Log("MoveState");
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
            UnityEngine.Debug.Log("MoveState out");
            Player.Instance.animator.SetBool("speed", false);
        }
    }
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
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
                LookAt(hit.point - Controller.transform.position);
            }
            attack();
            Player.Instance.weaponManager.Weapon?.Attack(this);
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