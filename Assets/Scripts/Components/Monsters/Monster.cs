using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{ 
    void Start()
    {
        OnStart();
    }

    void Update()
    {
        OnUpdate();
    }

    public virtual void OnStart()
    {

    }

    public virtual void OnUpdate()
    {
        // 상태 행동
        // detect player
    }

    public virtual void Idle()
    {
        // 정지 모션
    }

    public virtual void Move()
    {
        // 이동 (patrol)
    }
    
    public virtual void Attack()
    {
        // 기본 공격
    }

    public virtual void TakeDamage()
    {
        // 데미지 입음
    }

    public virtual void Death()
    {
        // 사망 (아이템 드랍)
    }
}
