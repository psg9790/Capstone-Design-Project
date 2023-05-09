using System.Collections;
using System.Collections.Generic;
using CharacterController;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public WeaponPositionData HandleData { get { return weaponPositionData; } }
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }
    
    public string Name { get { return _name; } }
    public float AttackDamage { get { return attackDamamge; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }


    [Header("weapon local transform")]
    [SerializeField] protected WeaponPositionData weaponPositionData;

    [Header("무기정보")] 
    [SerializeField] protected HitBox[] attack_effect;
    [SerializeField] protected HitBox[] skill_effect;
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;
    [SerializeField] public Transform shootpos;
    [SerializeField] protected string _name;
    [SerializeField] protected float attackDamamge;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;

    public void SetWeaponData(string name, float attackDamamge, float attackSpeed, float attackRange)
    {
        // weapon = new Item();
        // string sss = weapon.itemData.name;
        // weapon.itemOption.atk = attackDamamge;
        // weapon.itemOption.atk_speed = attackSpeed;
        
        // this._name = name;
        // this.attackDamamge = attackDamamge;
        // this.attackSpeed = attackSpeed;
        // this.attackRange = attackRange;
    }

    public abstract void Attack(BaseState state,Vector3 looking);
    public abstract void Skill();
    public abstract void StartAttack(int combo);
    public abstract void EndAttack();

}
