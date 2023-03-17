using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Monster
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Pattern1();
        Pattern2();
    }

    public override void Idle()
    {
        base.Idle();
    }

    public override void Move()
    {
        base.Move();
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
    }

    public override void Death()
    {
        base.Death();
    }

    public void Pattern1()
    {
        
    }

    public void Pattern2()
    {
        
    }
}
