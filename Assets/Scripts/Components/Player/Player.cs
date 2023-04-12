using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using CharacterController;

public class Player : MonoBehaviour
{
    public static Player Instance {get {return instance;}}
    public StateMachine stateMachine { get; private set; }
    public Rigidbody rigidbody { get; private set; }
    public Animator animator { get; private set; }
    public CapsuleCollider capsuleCollider { get; private set; }
    public WeaponManager weaponManager { get; private set; }
    
    public NavMeshAgent nav { get; private set; }
    private static Player instance;

    [SerializeField]
    private Transform rightHand;
    
    
    
    // Dash
    [Header("dash")]
    [SerializeField]
    public float dashPower = 10f; // 대쉬 거리
    [SerializeField]
    public float dashTetanyTime = 0.5f; // 대쉬 시간
    [SerializeField]
    public float dashCooltime = 1f; // 대쉬 쿨다운

    
    
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            weaponManager = new WeaponManager(rightHand);
            weaponManager.unRegisterWeapon = (weapon) => { Destroy(weapon); };
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            nav = GetComponent<NavMeshAgent>();
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
    }

    void Start()
    {
        InitStateMachine();
        nav.updateRotation = false;
    }

    void Update()
    {
        stateMachine?.UpdateState();
        
    }

    private void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }

    private void InitStateMachine() // 새로운 상태 추하갈 때 stateMachine.AddState(StateName.새로운상태, new 새로운상태State(controller)); 추가
    {
        
        PlayerController controller = GetComponent<PlayerController>();        
        stateMachine = new StateMachine(StateName.Idle, new IdleState(controller));
        stateMachine.AddState(StateName.move, new MoveState(controller));
        stateMachine.AddState(StateName.dash, new DashState(controller));
        stateMachine.AddState(StateName.attack, new AttackState(controller));
    }
    
    


}

