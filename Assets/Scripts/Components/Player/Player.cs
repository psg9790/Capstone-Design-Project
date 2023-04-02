using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    
    // state
    [Sirenix.OdinInspector.ReadOnly] 
    public PlayerState state = PlayerState.Idle;
    
    public int DashCount { get { return dashCount; } }

    [SerializeField] protected int dashCount;
    // move
    private NavMeshAgent nav;
    private Vector3 moveTarget;
    private bool isDodge = false;

    private bool isAttackReady = true;
    private float attackDelay;

    private Weapon equipWeapon;

    public void OnUpdateStat(int dashCount)
    {
        this.dashCount = dashCount;
    }
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.updateRotation = false;
    }

    void Update()
    {
        NavRotation();
        // attackDelay += Time.deltaTime;
        // 움직이는 상태이면
        if (state == PlayerState.Move)
        {
            Vector3 dist = moveTarget - transform.position;
            if (dist.magnitude <= 0.1f)
            {
                state = PlayerState.Idle;
                Animator anim = GetComponent<Animator>();
                anim.SetFloat("speed", 0);
            }
        }
        
    }

    public void attack()
    {
        // isAttackReady = equipWeapon.rate < attackDelay;
        if (state != PlayerState.Death && 
            state != PlayerState.Cc && 
            state != PlayerState.Attack &&
            state != PlayerState.Dash
            )
        {
            // equipWeapon.use();
            state = PlayerState.Attack;
            nav.ResetPath();
            Animator anim = GetComponent<Animator>();
            anim.SetFloat("speed", 0);
            anim.SetTrigger("attack");
            Invoke("attackend",0.5f);

            attackDelay = 0;
        }
    }

    public void attackend()
    {
        state = PlayerState.Idle;
    }
    public void Move(Vector3 pos)
    {
        if (state != PlayerState.Death && 
            state != PlayerState.Cc && 
            state != PlayerState.Attack &&
            state != PlayerState.Dash)
        {
            state = PlayerState.Move;
            nav.SetDestination(pos);
            moveTarget = pos;

            Animator anim = GetComponent<Animator>();
            anim.SetFloat("speed", 1.1f);
        }
    }

    public void Dodge()
    {
        if (state != PlayerState.Death && 
            state != PlayerState.Cc && 
            state != PlayerState.Attack)
        {
            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("doDodge");
            isDodge = true;
            state = PlayerState.Dash;
            
        }
        
    }

    public void Dodgeout()
    {
        isDodge = false;
        state = PlayerState.Idle;
    }
    void NavRotation()
    {
        if (!nav.hasPath)
            return;
        
        Vector2 forward = new Vector2(transform.position.z, transform.position.x);
        Vector2 steeringTarget = new Vector2(nav.steeringTarget.z, nav.steeringTarget.x);
    
        //방향을 구한 뒤, 역함수로 각을 구한다.
        Vector2 dir = steeringTarget - forward;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    
        //방향 적용
        transform.eulerAngles = Vector3.up * angle;
    }
}

public enum PlayerState
{
    Idle, 
    Move,
    Dash,
    NDash,
    Attack,
    Interact,
    Cc,
    Death
}
