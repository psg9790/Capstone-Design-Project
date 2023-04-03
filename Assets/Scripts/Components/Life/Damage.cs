using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public float damage;
    
    // cc 정보
    public CC_type ccType;

    public Damage(float damage, CC_type ccType)
    {
        this.damage = damage;
        this.ccType = ccType;
    }
}

public enum CC_type
{
    None,
    Stun,
    Knockback
}
