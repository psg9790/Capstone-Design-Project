using System.Collections;
using System.Collections.Generic;
using CharacterController;
using Sirenix.OdinInspector;
using UnityEngine;

public class Sword1 : BaseWeapon
{
    public GameObject attack;

    public override void Attack(BaseState state)
    {
        GameObject effect = Instantiate(attack);
        effect.transform.position = Player.Instance.effectGenerator.position;
        effect.GetComponent<ParticleSystem>().Play();
    }

    public override void Skill(BaseState state)
    {
    }

}
