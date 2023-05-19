using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using CharacterController;
using Unity.VisualScripting;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{
    public Player player { get; private set; }
    [Header("Item search radius")]
    public float radius = 1f;
    
    // private bool rightClickHold = false;
    // private bool leftClick = false;
    // private bool SpaceClick = false;

    private Coroutine dashCoolTimeCoroutine;
    
    // 스킬 입력 키
    public int skillnum = -1;
    
    // public float dashCooldown = Player.Instance.dashCooltime; // 대쉬 쿨다운

    public bool isDashing = false;
    public bool isAttack = false;
    public bool isSkill = false;
    
    private bool isDashCollTime = false;

    private void Start()
    {
        if (InputManager.Instance != null)
        {
            // InputManager.Instance.AddPerformed(InputType.RightClick, OnMove);
            InputManager.Instance.AddPerformed(InputKey.LeftClick, LeftClickPerformed);
            InputManager.Instance.AddPerformed(InputKey.RightClick, RighClickPerformed);
            // InputManager.Instance.AddCanceled(InputKey.RightClick, RighClickCanceled);
            InputManager.Instance.AddPerformed(InputKey.SpaceClick, SpaceClickPerformed);
            InputManager.Instance.AddPerformed(InputKey.QClick, QClickPerformed);
            InputManager.Instance.AddPerformed(InputKey.WClick, WClickPerformed);
            InputManager.Instance.AddPerformed(InputKey.EClick, EClickPerformed);
            InputManager.Instance.AddPerformed(InputKey.RClick, RClickPerformed);

            
        }
    
        player = GetComponent<Player>();
        
    }

    private void OnDestroy() // 플레이어 지울시 이벤트 삭제
    {
        InputManager.Instance.RemovePerformed(InputKey.LeftClick, LeftClickPerformed);
        InputManager.Instance.RemovePerformed(InputKey.RightClick, RighClickPerformed);
        InputManager.Instance.RemovePerformed(InputKey.SpaceClick, SpaceClickPerformed);
        InputManager.Instance.RemovePerformed(InputKey.QClick, QClickPerformed);
        InputManager.Instance.RemovePerformed(InputKey.WClick, WClickPerformed);
        InputManager.Instance.RemovePerformed(InputKey.EClick, EClickPerformed);
        InputManager.Instance.RemovePerformed(InputKey.RClick, RClickPerformed);
    }

    private void Update()
    {
        // searchItem();
    }
    // 마우스 좌클릭 공격
    void LeftClickPerformed(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (isAttack)
        {
            Player.Instance.animator.SetTrigger("attack");
        }
        if (!isDashing && !isAttack && !isSkill)
        {
            player.stateMachine.ChangeState(StateName.attack);
        }
        
    }

    void QClickPerformed(InputAction.CallbackContext context)
    {
        if(isSkill) return;
        skillnum = 0;
        player.stateMachine.ChangeState(StateName.skill);
    }
    void WClickPerformed(InputAction.CallbackContext context)
    {
        if(isSkill) return;
        skillnum = 1;
        player.stateMachine.ChangeState(StateName.skill);
    }
    void EClickPerformed(InputAction.CallbackContext context)
    {
        if(isSkill) return;
        skillnum = 2;
        player.stateMachine.ChangeState(StateName.skill);
    }
    void RClickPerformed(InputAction.CallbackContext context)
    {
        if(isSkill) return;
        skillnum = 3;
        player.stateMachine.ChangeState(StateName.skill);
    }
    
    // 마우스 우클릭 이동 
    void RighClickPerformed(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        // 이동하면 안되는 조건문 추가
        if (!isDashing && !isSkill)
        {
            player.stateMachine.ChangeState(StateName.move);
            
        }
    }
    
    // 스페이스바 대쉬
    void SpaceClickPerformed(InputAction.CallbackContext context)
    {
        if (!isDashing && !isDashCollTime)
        {
            isDashing = true;
            isDashCollTime = true;
            player.stateMachine.ChangeState(StateName.dash);
            dashCoolTimeCoroutine = StartCoroutine(DashCoolTimeCoroutine());
        }
    }
    protected void LookAt(Vector3 direction) // 플레이어 보는 방향 설정
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(direction);
            transform.rotation = targetAngle;
        }
    }

    private IEnumerator DashCoolTimeCoroutine() // 대쉬 쿨타임 계산 코루틴
    {
        float currentTime = 0f;
        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= Player.Instance.dashCooltime)
            {
                break;
            }

            yield return null;
        }

        isDashCollTime = false;
    }
    public void EndAttack()
    {
        if (InputManager.Instance.GetAction(InputKey.RightClick).IsPressed())
        {
            Player.Instance.stateMachine.ChangeState(StateName.move);
        }
        else
        {
            Player.Instance.stateMachine.ChangeState(StateName.Idle);
        }
    }

    public void searchItem()
    {
        Collider[] colls = Physics.OverlapSphere(player.transform.position, radius,1 << LayerMask.NameToLayer("Item"));
        
        foreach (Collider coll in colls)
        {
            Item drop = coll.gameObject.GetComponent<DroppedItem>().item;
            Debug.Log(drop.itemData.itemName);
            
        }
    }

}
