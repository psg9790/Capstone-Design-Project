using System;
using System.Collections;
using System.Collections.Generic;
using CharacterController;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class Wand1 : BaseWeapon
{
    private Coroutine buff;
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
        // Player.Instance.animator.ResetTrigger("attack");
    }
    public override void Skill(int i)
    {
        HitBox hitBox = Instantiate(skill_effect[i]);
        if (i == 1)
        {
            Player.Instance.heart.ATK_SPEED_CHANGE(1);
            buff = StartCoroutine(CoolTimeCoroutine());
        }
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
    
    private IEnumerator CoolTimeCoroutine()
    {
        float currentTime = 0f;
        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 5)
            {
                break;
            }

            yield return null;
        }
        
        Player.Instance.heart.ATK_SPEED_CHANGE(-1);
    }

}
