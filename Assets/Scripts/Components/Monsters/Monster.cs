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
        // 기본 정지 모션
    }

    public virtual void Patrol()
    {
        // 순찰 이동
    }
    
    public virtual void FindPlayer()
    {
        // 기본: 정면 기준 원뿔 범위 탐색
    }
    
    public virtual void ChasePlayer()
    {
        // 일정 거리까지는 플레이어 따라감
    }

    public virtual void R2B()
    {
        // 원래 있던 지점으로 복귀
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
