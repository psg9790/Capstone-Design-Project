using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    
    // state
    [Sirenix.OdinInspector.ReadOnly] 
    public State state = State.Idle;
    
    public int DashCount { get { return dashCount; } }

    [SerializeField] protected int dashCount;
    // move
    public NavMeshAgent nav;
    private Vector3 moveTarget;
    private bool isDodge = false;

    private bool isAttackReady = true;
    private float attackDelay;

    private _Weapon equipWeapon;

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
        attackDelay += Time.deltaTime;
        NavRotation();
        // attackDelay += Time.deltaTime;
        // 움직이는 상태이면
        if (state == State.Move)
        {
            Vector3 dist = moveTarget - transform.position;
            if (dist.magnitude <= 0.1f)
            {
                state = State.Idle;
                Animator anim = GetComponent<Animator>();
                anim.SetFloat("speed", 0);
            }
        }
        else if (state == State.Attack)
        {
            if (attackDelay > 0.5f)
            {
                state = State.Idle;
            }
        }
    }

    public void attack()
    {
        // isAttackReady = equipWeapon.rate < attackDelay;
        if (state != State.Death && 
            state != State.Cc &&
            state != State.Dash
            )
        {
            // equipWeapon.use();
            state = State.Attack;
            nav.ResetPath();
            Animator anim = GetComponent<Animator>();
            anim.SetFloat("speed", 0);
            // anim.SetTrigger("attack");
            anim.SetTrigger("Bow_attack");

            attackDelay = 0;
        }
    }

    public void attackend()
    {
        state = State.Idle;
    }
    public void Move(Vector3 pos)
    {
        if (state != State.Death && 
            state != State.Cc && 
            state != State.Attack &&
            state != State.Dash &&
            !EventSystem.current.IsPointerOverGameObject ())
        {
            state = State.Move;
            nav.SetDestination(pos);
            moveTarget = pos;

            Animator anim = GetComponent<Animator>();
            anim.SetFloat("speed", 1.1f);
        }
    }

    public void Dodge()
    {
        if (state != State.Death && 
            state != State.Cc && 
            state != State.Attack)
        {
            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("doDodge");
            isDodge = true;
            state = State.Dash;
            
        }
        
    }

    public void Dodgeout()
    {
        isDodge = false;
        state = State.Idle;
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

public enum State
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
