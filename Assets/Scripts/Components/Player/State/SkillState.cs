using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CharacterController
{
    public class SkillState : BaseState
    {
        private bool[] isCollTime = {false,false,false,false};
        
        private Coroutine coolTimeCoroutine;
        public SkillState(PlayerController controller) : base(controller)
        {
            
        }
        public override void OnEnterState()
        {
            // UnityEngine.Debug.Log("Skill enter");
            Player.Instance.nav.ResetPath();
            int num = Controller.skillnum;
            if (!isCollTime[num] && !Controller.isSkill)
            {
                Controller.isSkill = true;
                isCollTime[num] = true;
                skill(num);
                
                coolTimeCoroutine = CoolTimeHelper.StartCoroutine(CoolTimeCoroutine(num));
            }
            else
            {
                Debug.Log(num+"번 째 스킬 쿨타임중...");
                Player.Instance.stateMachine.ChangeState(StateName.Idle);
            }

        }

        public void skill(int num)
        {
            Ray ray = CameraController.Instance.cam.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            RaycastHit hit;
            Vector3 looking;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
                looking = hit.point - Player.Instance.transform.position;
                Player.Instance.weaponManager.atk_pos = looking;
                LookAt(hit.point - Player.Instance.transform.position);
            }
            Player.Instance.nav.ResetPath();
            
            Player.Instance.animator.SetInteger("skillnum", num);
            Player.Instance.animator.SetTrigger("skill");
        }

        
        public override void OnUpdateState()
        {
            
        }

        public override void OnFixedUpdateState()
        {
            
        }

        
        public override void OnExitState()
        {
            Controller.isSkill = false;
            // Player.Instance.animator.SetTrigger("resetanim");
            Player.Instance.animator.SetInteger("skillnum",-1);
        }
        protected void LookAt(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                Quaternion targetAngle = Quaternion.LookRotation(direction);
                Controller.transform.rotation = targetAngle;
            }
        }
        private IEnumerator CoolTimeCoroutine(int i) // 쿨타임 계산
        {
            float currentTime = 0f;
            while (true)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= Player.Instance.weaponManager.CoolTime[i])
                {
                    break;
                }

                yield return null;
            }

            isCollTime[i] = false;
        }
        
        
        
    }
}