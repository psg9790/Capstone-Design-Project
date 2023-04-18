using System.Collections;
using System.Collections.Generic;
using CharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

public class Sword1 : BaseWeapon
{
    public GameObject attack;
    public GameObject effect;
    public Vector3 mouselook;

    public override void Attack(BaseState state)
    {
        
    }

    public void OnStartAttack()
    {
        
        effect = Instantiate(attack);
        effect.transform.position = Player.Instance.effectGenerator.position;
        // effect.transform.rotation = 
        effect.GetComponent<ParticleSystem>().Play();
    }
    public void OnEndAttack()
    {
        
        
        // effect.transform.rotation = 
        effect.GetComponent<ParticleSystem>().Stop();
    }
    public override void Skill(BaseState state)
    {
    }

}
