using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private Player player;
    private Camera cam;
    private bool rightClickHold = false;
    private bool leftClick = false;
    private bool SpaceClick = false;
    //이동
    private const float RAY_DISTANCE = 2f;
    private RaycastHit slopeHit;
//    private int groundLayer = 1 << LayerMask.NameToLayer("Walkable");

    //구르기
    protected Vector3 inputDirection;
    protected Vector3 calculatedDirection;
    protected Vector3 gravity;
    private bool isOnslope;
    private bool isGrounded;
    
    [Header("dash")] 
    [SerializeField]
    protected float dashPower;
    [SerializeField]
    protected float dashForwardRollTime;
    [SerializeField]
    protected float dashReInputTime;
    [SerializeField]
    protected float dashTetanytime;
    [SerializeField]
    protected float dashCoolTime;

    private WaitForSeconds DASH_FORWARD_ROOL_TIME;
    private WaitForSeconds DASH_RE_INPUT_TIME;
    private WaitForSeconds DASH_TETANY_TIME;
    private Coroutine dashCoroutine;
    private Coroutine dashCoolTimeCoroutine;
    private int currentDashCount;
    
    [Header("dash")]
    [SerializeField]
    public float dashDistance = 10f; // 대쉬 거리
    [SerializeField]
    public float dashDuration = 0.5f; // 대쉬 시간
    [SerializeField]
    public float dashCooldown = 1f; // 대쉬 쿨다운

    private NavMeshAgent navMeshAgent;
    private bool isDashing;
    private float dashStartTime;
    private Vector3 dashDirection;

    
    private void Start()
    {
        if (InputManager.Instance != null)
        {
            // InputManager.Instance.AddPerformed(InputType.RightClick, OnMove);
            InputManager.Instance.AddPerformed(InputKey.LeftClick, LeftClickPerformed);
            InputManager.Instance.AddPerformed(InputKey.RightClick, RighClickPerformed);
            InputManager.Instance.AddCanceled(InputKey.RightClick, RighClickCanceled);
            InputManager.Instance.AddPerformed(InputKey.SpaceClick, SpaceClickPerformed);
        }
    
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GetComponent<Player>();
        cam = Camera.main;
        
        // 대쉬
        DASH_FORWARD_ROOL_TIME = new WaitForSeconds(dashForwardRollTime);
        DASH_RE_INPUT_TIME = new WaitForSeconds(dashReInputTime);
        DASH_TETANY_TIME = new WaitForSeconds(dashTetanytime);
    }

    private void Update()
    {
        if (rightClickHold)
        {
            Ray ray = cam.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
                player.Move(hit.point);
            }
        }
        else if (isDashing)
        {
            // 일정 시간이 지난 후에 NavMeshAgent 활성화
            if (Time.time >= dashStartTime + dashDuration)
            {
                isDashing = false;
                navMeshAgent.enabled = true;
                player.state = PlayerState.Idle;
            }
            else
            {
                // 플레이어 이동
                Vector3 dashVelocity = dashDirection * (dashDistance / dashDuration);
                transform.position += dashVelocity * Time.deltaTime;
            }
        }
    }

    void LeftClickPerformed(InputAction.CallbackContext context)
    {
        Ray ray = cam.ScreenPointToRay(InputManager.Instance.GetMousePosition());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
        {
            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
            LookAt(hit.point - transform.position);
        }
        player.attack();
    }
    void RighClickPerformed(InputAction.CallbackContext context)
    {
        rightClickHold = true;
    }
    
    void RighClickCanceled(InputAction.CallbackContext context)
    {
        rightClickHold = false;
    }

    //public bool IsOnslope()
    //{
    //    Ray ray = new Ray(transform.position, Vector3.down);
    //    if (Physics.Raycast(ray, out slopeHit, RAY_DISTANCE, groundLayer))
    //    {
    //        var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
    //        return angle != 0f && angle < maxSlopeAngle;
    //    }
    //}
    //protected Vector3 GetDirection(float currentMoveSpeed)
    //{
    //    isOnslope = IsOnslope();
    //    isGrounded = IsGrounded();
    //    Vector3 calculatedDirection = calculateNextFrameGroundAngle()
    //}

    protected void LookAt(Vector3 direction)
    {
        
        if (direction != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(direction);
            Rigidbody rg = GetComponent<Rigidbody>();
            transform.rotation = targetAngle;
        }
    }
    private IEnumerator DashCoroutine()
    {
        Vector3 LookAtDirection = (inputDirection == Vector3.zero) ? transform.forward : inputDirection;
        Vector3 dashDirection = (calculatedDirection == Vector3.zero) ? transform.forward : calculatedDirection;
        
        Animator anim = GetComponent<Animator>();
        
        
        anim.SetFloat("speed",0f);
        anim.SetTrigger("doDodge");
        Rigidbody rg = GetComponent<Rigidbody>();
        rg.velocity = dashDirection * dashPower; // + speed

        yield return DASH_FORWARD_ROOL_TIME;
        player.state = (player.DashCount > 1 && currentDashCount < player.DashCount)
            ? PlayerState.NDash
            : PlayerState.Dash;

        yield return DASH_RE_INPUT_TIME;
        rg.velocity = Vector3.zero;
        // anim.SetFloat("speed",1f);
        yield return DASH_TETANY_TIME;
        player.state = PlayerState.Idle;

        dashCoolTimeCoroutine = StartCoroutine(DashCoolTimeCoroutine());
    }

    private IEnumerator DashCoolTimeCoroutine()
    {
        float currentTime = 0f;
        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= dashCoolTime)
            {
                break;
            }

            yield return null;
        }

        if (currentDashCount == player.DashCount)
        {
            currentDashCount = 0;
        }
    }
    
    void SpaceClickPerformed(InputAction.CallbackContext context)
    {
        
        bool isAvailableDash = player.state != PlayerState.Dash && currentDashCount < player.DashCount;
        if (isAvailableDash)
        {
            player.state = PlayerState.Dash;
            Ray ray = cam.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
                LookAt(hit.point - transform.position);
            }
            currentDashCount++;
            if (dashCoroutine != null && dashCoolTimeCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
                StopCoroutine(dashCoolTimeCoroutine);
            }

            dashCoroutine = StartCoroutine(DashCoroutine());
            
            
            
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 clickPosition = hit.point;

                // 현재 위치와 대쉬할 방향 벡터 계산
                Vector3 currentPosition = transform.position;
                Vector3 dashVector = clickPosition - currentPosition;
                dashVector.y = 0f;
                float _dashDistance = dashVector.magnitude;
                
                // 대쉬할 방향 벡터 저장
                dashDirection = dashVector.normalized;
                
                // NavMeshAgent 비활성화
                navMeshAgent.enabled = false;
                
                // 대쉬 시작
                isDashing = true;
                dashStartTime = Time.time;
                
                // // 대쉬 거리 이내인지 확인
                // if (_dashDistance <= this.dashDistance)
                // {
                //     // 대쉬할 방향 벡터 저장
                //     dashDirection = dashVector.normalized;
                //
                //     // NavMeshAgent 비활성화
                //     navMeshAgent.enabled = false;
                //
                //     // 대쉬 시작
                //     isDashing = true;
                //     dashStartTime = Time.time;
                // }
            }
        }
    }

}
