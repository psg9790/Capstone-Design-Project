using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public float damage;
    public Vector3 pos; // 데미지 출처 위치, 피격 시 그 방향을 바라보도록 할 때 사용
    
    // cc 정보
    public CC_type ccType;
    public float ccPower;

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
