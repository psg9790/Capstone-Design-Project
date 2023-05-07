using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using CharacterController;
using Unity.VisualScripting;


public class PlayerController : MonoBehaviour
{
    public Player player { get; private set; }
    [Header("Item search radius")]
    public float radius = 1f;
    
    public Camera cam;
    // private bool rightClickHold = false;
    // private bool leftClick = false;
    // private bool SpaceClick = false;

    private Coroutine dashCoolTimeCoroutine;
    
    
    
    // public float dashCooldown = Player.Instance.dashCooltime; // 대쉬 쿨다운

    public bool isDashing = false;
    public bool isAttack = false;
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
            InputManager.Instance.AddPerformed(InputKey.QClick, SkillClickPerformed);
        }
    
        player = GetComponent<Player>();
        cam = Camera.main;
        
    }

    private void Update()
    {
        searchItem();
    }
    // 마우스 좌클릭 공격
    void LeftClickPerformed(InputAction.CallbackContext context)
    {
        if (!isDashing && !isAttack)
        {
            player.stateMachine.ChangeState(StateName.attack);
            // rightClickHold = true;
            // player.stateMachine.OnEnterState();
        }
        // player.stateMachine.ChangeState();
        // Ray ray = cam.ScreenPointToRay(InputManager.Instance.GetMousePosition());
        // RaycastHit hit;
        // if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
        // {
        //     Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
        //     LookAt(hit.point - transform.position);
        // }
        // player.attack();
    }

    void SkillClickPerformed(InputAction.CallbackContext context)
    {
        player.stateMachine.ChangeState(StateName.skill);
    }
    
    
    
    // 마우스 우클릭 이동 
    void RighClickPerformed(InputAction.CallbackContext context)
    {
        // 이동하면 안되는 조건문 추가
        if (!isDashing)
        {
            player.stateMachine.ChangeState(StateName.move);
            // rightClickHold = true;
            // player.stateMachine.OnEnterState();
        }
    }
    
    // void RighClickCanceled(InputAction.CallbackContext context)
    // {
    //     rightClickHold = false;
    // }
    
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
        Collider[] colls = Physics.OverlapSphere(player.transform.position, radius,1 << LayerMask.NameToLayer("ITEM"));
        
        foreach (Collider coll in colls)
        {
            Item drop = coll.gameObject.GetComponent<DroppedItem>().item;
            Debug.Log(drop.itemData.itemName);
            // InventoryManager.Instance.addItem(drop);
            // Destroy(coll.gameObject);
        }
    }

}
