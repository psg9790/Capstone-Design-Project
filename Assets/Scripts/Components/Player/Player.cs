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
    public static Player Instance
    {
        get { return instance; }
    }

    public StateMachine stateMachine { get; private set; }
    public Rigidbody rigidbody { get; private set; }
    public Animator animator { get; private set; }
    public CapsuleCollider capsuleCollider { get; private set; }
    public WeaponManager weaponManager { get; private set; }
    public Heart heart { get; private set; }

    public NavMeshAgent nav { get; private set; }
    private static Player instance;

    [SerializeField] private Transform rightHand;

    public Transform effectGenerator;

    public RuntimeAnimatorController BaseAnimator;

    public GameObject baseWeapon;

    // Dash
    [Header("dash")] [SerializeField] public float dashDistance = 10f; // 대쉬 거리
    [SerializeField] public float dashDuration = 0.5f; // 대쉬 시간
    [SerializeField] public float dashCooltime = 5f; // 대쉬 쿨다운


    private void Awake()
    {
        if (instance == null)
        {
            UnityEngine.Debug.Log("Player Awake");
            instance = this;
            weaponManager = new WeaponManager(rightHand);
            weaponManager.unRegisterWeapon = (weapon) => { Destroy(weapon); };
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            nav = GetComponent<NavMeshAgent>();
            heart = GetComponent<Heart>();
            // DontDestroyOnLoad(gameObject);
            return;
        }

        DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    void Start()
    {
        InitStateMachine();
        nav.updateRotation = false;

        weaponManager.SetWeapon(baseWeapon);
        heart.PlayerItemEquip();
    }

    void Update()
    {
        stateMachine?.UpdateState();
        UpdateStat();
    }

    private void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }

    private void
        InitStateMachine() // 새로운 상태 추하갈 때 stateMachine.AddState(StateName.새로운상태, new 새로운상태State(controller)); 추가
    {
        PlayerController controller = GetComponent<PlayerController>();
        stateMachine = new StateMachine(StateName.Idle, new IdleState(controller));
        stateMachine.AddState(StateName.move, new MoveState(controller));
        stateMachine.AddState(StateName.dash, new DashState(controller));
        stateMachine.AddState(StateName.attack, new AttackState(controller));
        stateMachine.AddState(StateName.skill, new SkillState(controller));
        stateMachine.AddState(StateName.stiff, new StiffState(controller));
        stateMachine.AddState(StateName.death, new DeathState(controller));
    }

    public void UpdateStat()
    {
        if (Inventory.instance != null)
        {
            if (Inventory.instance.gameObject.activeSelf)
            {
                heart.PlayerItemEquip();
            }
        }

        
    }


    public void OnStartAttack(int combo)
    {
        weaponManager.Weapon?.StartAttack(combo);
    }

    public void OnEndAttack()
    {
        weaponManager.Weapon?.EndAttack();
        stateMachine.ChangeState(StateName.Idle);
    }

    public void OnStartSkill(int i) // i = skill input
    {
        weaponManager.Weapon?.Skill(i);
    }

    public void OnEndSkill()
    {
        Debug.Log("endskill");
        stateMachine?.ChangeState(StateName.Idle);
    }

    public void OnDeath()
    {
        stateMachine.ChangeState(StateName.death);
        capsuleCollider.enabled = false;
    }
    
    [Button]
    public void Respawn()
    {
        stateMachine?.ChangeState(StateName.Idle);
        capsuleCollider.enabled = true;
        heart.RestoreAll_CUR_HP();
    }
    
}