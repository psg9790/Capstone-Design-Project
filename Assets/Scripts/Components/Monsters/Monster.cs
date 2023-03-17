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
        
    }

    public virtual void Idle()
    {
        
    }

    public virtual void Move()
    {
        
    }
    
    public virtual void Attack()
    {
        
    }

    public virtual void TakeDamage()
    {
        
    }

    public virtual void Death()
    {
        
    }
}
