using System.Collections;
using System.Collections.Generic;
using CharacterController;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public WeaponPositionData HandleData { get { return weaponPositionData; } }
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }
    
    


    [Header("weapon local transform")]
    [SerializeField] protected WeaponPositionData weaponPositionData;
    [SerializeField] public Transform shootpos;

    [Header("무기정보")] 
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;
    [SerializeField] protected HitBox[] attack_effect;
    [SerializeField] protected HitBox[] skill_effect;
    [SerializeField] public float QCoolTime;
    [SerializeField] public float WCoolTime;
    [SerializeField] public float ECoolTime;
    [SerializeField] public float RCoolTime;

    
    public abstract void Attack(BaseState state,Vector3 looking);
    public abstract void Skill(int i); // i = skill key input
    public abstract void EndSkill();
    public abstract void StartAttack(int combo);
    public abstract void EndAttack();

    public void Bullet_Play(HitBox hitBox)
    {
        Vector3 pos = shootpos.position;
        Vector3 rot = Player.Instance.weaponManager.atk_pos;
        hitBox.BulletParticle_Play(Player.Instance.heart,pos,rot);
    }
    
}
