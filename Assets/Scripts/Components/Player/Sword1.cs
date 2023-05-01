using System;
using System.Collections;
using System.Collections.Generic;
using CharacterController;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class Sword1 : BaseWeapon
{
    
    

    private void Awake()
    {
        
        // for (int i = 0; i < Player.Instance.effectGenerator.childCount; i++)
        // {
        //     if (Player.Instance.effectGenerator.GetChild(i).name == _name+"eff")
        //     {
        //         eff = i;
        //         
        //     }
        //
        //     if (Player.Instance.effectGenerator.GetChild(i).name == _name+"Skilleff")
        //     {
        //         seff = i;
        //     }
        // }
    }

    public override void Attack(BaseState state,Vector3 looking)
    {
       
    }

    public override void StartAttack()
    {
        HitBox hitBox = Instantiate(attack_effect);
        hitBox.Particle_Play(Player.Instance.heart);
        // effect = Instantiate(attack);
        // effect.transform.position = Player.Instance.effectGenerator.position;
        //
        // effect.transform.rotation = Player.Instance.transform.rotation;
        // effect.transform.eulerAngles += new Vector3(0f, -90f, 0f);
        // effect.transform.eulerAngles += new Vector3(-20f, 0f, 0f);
        // effect.SetActive(true);
        // effect.GetComponent<ParticleSystem>().Play();
        // Player.Instance.effectGenerator.GetChild(eff).gameObject.SetActive(true);
        // Player.Instance.effectGenerator.GetChild(eff).gameObject.GetComponent<ParticleSystem>().Play();

    }
    public override void EndAttack()
    {
        // effect.transform.rotation = 
        // effect.GetComponent<ParticleSystem>().Stop();
        // effect.SetActive(false);
        // Destroy(effect);
        // Player.Instance.effectGenerator.GetChild(eff).gameObject.GetComponent<ParticleSystem>().Stop();
        // Player.Instance.effectGenerator.GetChild(eff).gameObject.SetActive(false);
    }
    public override void Skill()
    {
        // Player.Instance.effectGenerator.GetChild(seff).gameObject.SetActive(true);
        // Player.Instance.effectGenerator.GetChild(seff).gameObject.GetComponent<ParticleSystem>().Play();
        
        
    }

    

}
