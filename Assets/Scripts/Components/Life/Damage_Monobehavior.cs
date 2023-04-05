using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Damage_Monobehavior : MonoBehaviour
{
    public Damage Damage;
    public float dmg;
    public CC_type ccType;
    
    private void Awake()
    {
        Damage = new Damage(dmg, ccType);
    }

    public void CreateDamage()
    {
        
    }
}
