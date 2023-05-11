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

    public override void StartAttack(int combo)
    {
        HitBox hitBox = Instantiate(attack_effect[combo]);

        if (hitBox.isBullet)
        {
            Bullet_Play(hitBox);
        }
        else
        {
            hitBox.Particle_Play(Player.Instance.heart);
        }
        

    }
    public override void EndAttack()
    {
        // Player.Instance.stateMachine?.CurrentState?.OnExitState();
    }
    public override void Skill(int i)
    {
        HitBox hitBox = Instantiate(skill_effect[i]);
        if (hitBox.isBullet)
        {
            Bullet_Play(hitBox);
        }
        else
        {
            hitBox.Particle_Play(Player.Instance.heart);
        }
    }
    public override void EndSkill()
    {
        Player.Instance.animator.SetInteger("skillnum",-1);
    }
}
