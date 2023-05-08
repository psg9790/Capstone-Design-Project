using System;
using System.Collections;
using System.Collections.Generic;
using CharacterController;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class Gun1 : BaseWeapon
{
    public override void Attack(BaseState state,Vector3 looking)
    {
       
    }

    public override void StartAttack()
    {
        HitBox hitBox = Instantiate(attack_effect);

        Vector3 pos = shootpos.position;
        Vector3 rot = Player.Instance.weaponManager.atk_pos;
        /*Vector3 rot = Player.Instance.weaponManager.atk_pos - pos;
        rot.y = 0f;*/
        
        hitBox.BulletParticle_Play(Player.Instance.heart,pos,rot);
        

    }
    public override void EndAttack()
    {
        Player.Instance.stateMachine?.CurrentState?.OnExitState();
    }
    public override void Skill()
    {
        HitBox hitBox = Instantiate(skill_effect);
        Vector3 pos = shootpos.position;
        Vector3 rot = Player.Instance.weaponManager.atk_pos;
        hitBox.BulletParticle_Play(Player.Instance.heart,pos,rot);
        
        
        
    }
}
