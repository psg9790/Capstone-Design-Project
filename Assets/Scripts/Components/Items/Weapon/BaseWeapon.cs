using System.Collections;
using System.Collections.Generic;
using CharacterController;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public int ComboCount { get; set; }
    public WeaponPositionData HandleData { get { return weaponPositionData; } }
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }
    
    public string Name { get { return _name; } }
    public float AttackDamage { get { return attackDamamge; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }


    [Header("weapon local transform")]
    [SerializeField] protected WeaponPositionData weaponPositionData;

    [Header("무기정보")] 
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;
    [SerializeField] protected string _name;
    [SerializeField] protected float attackDamamge;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;

    public void SetWeaponData(string name, float attackDamamge, float attackSpeed, float attackRange)
    {
        this._name = name;
        this.attackDamamge = attackDamamge;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
    }

    public abstract void Attack(BaseState state);
    public abstract void Skill(BaseState state);

}
