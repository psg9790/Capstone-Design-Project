using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Damage
{
    public float damage;
    // public Vector3 pos; // 데미지 출처 위치, 피격 시 그 방향을 바라보도록 할 때 사용
    
    // cc 정보
    public CC_type ccType;
    public float ccPower;
    public bool isCritical;

    public Damage(float damage, bool isCritical)
    {
        this.damage = damage;
        this.isCritical = isCritical;
        this.ccType = CC_type.None;
        this.ccPower = 0;
    }
    
    public Damage(float damage, bool isCritical, CC_type ccType, float ccPower)
    {
        this.damage = damage;
        this.isCritical = isCritical;
        this.ccType = ccType;
        this.ccPower = ccPower;
    }
}

public enum CC_type
{
    None,
    Stun,
    Knockback,
    Stiff
}
