using System;
using System.Collections;
using System.Collections.Generic;
using CharacterController;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class Sword1 : BaseWeapon
{

    public override void Attack(BaseState state,Vector3 looking)
    {
       
    }

    public override void StartAttack(int combo)
    {
        
        HitBox hitBox = Instantiate(attack_effect[combo]);
        hitBox.Particle_Play(Player.Instance.heart);
        

    }
    public override void EndAttack()
    {
        // Player.Instance.animator.ResetTrigger("attack");
    }
    public override void Skill()
    {
        HitBox hitBox = Instantiate(skill_effect[1]);
        hitBox.Particle_Play(Player.Instance.heart);
       
        
        
    }

    

}
