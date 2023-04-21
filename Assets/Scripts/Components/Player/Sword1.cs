using System.Collections;
using System.Collections.Generic;
using CharacterController;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class Sword1 : BaseWeapon
{
    public GameObject attack;
    private GameObject effect;
    public Vector3 mouselook;

    public override void Attack(BaseState state,Vector3 looking)
    {
        mouselook = looking;
    }

    public override void StartAttack()
    {
        
        effect = Instantiate(attack);
        effect.transform.position = Player.Instance.effectGenerator.position;

        effect.transform.rotation = Player.Instance.transform.rotation;
        effect.transform.eulerAngles += new Vector3(0f, -90f, 0f);
        effect.transform.eulerAngles += new Vector3(-20f, 0f, 0f);
        
        effect.GetComponent<ParticleSystem>().Play();
    }
    public override void EndAttack()
    {
        // effect.transform.rotation = 
        effect.GetComponent<ParticleSystem>().Stop();
        Destroy(effect);
    }
    public override void Skill(BaseState state)
    {
    }

}
