using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public LayerMask targetMask;
    // public Collider col;
    public ParticleSystem particle;

    private HashSet<string> hitHash = new HashSet<string>();
    [ShowInInspector] private Damage damage;


    public void Particle_Play()
    {
        ClearHash();
        particle.Play();
    }

    public void Particle_Stop()
    {
        particle.Stop();
    }

    private void OnParticleCollision(GameObject other)
    {
        UnityEngine.Debug.Log(other.gameObject.transform.root.name);
    }

    public void SetDamage(Damage _damage)
    {
        damage = _damage;
    }

    [Button]
    public void ClearHash()
    {
        hitHash.Clear();
    }
    
    // public void COLLIDER_ON(Damage _damage)
    // {
    //     damage = _damage;
    //     col.enabled = true;
    //     ClearHash();
    // }
    //
    // public void COLLIDER_OFF()
    // {
    //     col.enabled = false;
    // }
    //
    // private void OnTriggerEnter(Collider other)
    // {
    //     // other.gameObject.layer는 레이어 인덱스 (ex. 7)
    //     // targetMask는 인덱스로 시프트까지 계산된 값 (ex. 128)
    //     // 이상한데..
    //     if ((1 << other.gameObject.layer) == targetMask)
    //     {
    //         if (!hitHash.Contains(other.transform.root.name)) // 최상위부모 이름,,, 히트한 타겟이 해싱되어 있으면 다시 타격 x 
    //         {
    //             // Debug.Log(transform.root.name + " attacks " + other.transform.root.name);
    //             hitHash.Add(other.transform.root.name); // 히트한 타겟 해싱
    //             if (other.transform.root.TryGetComponent<Heart>(out Heart _heart))
    //             {
    //                 Vector3 dir = other.transform.position - transform.position;
    //                 dir.y = 0;
    //                 _heart.Take_Damage(damage, dir.normalized);
    //             }
    //             else
    //             {
    //                 Debug.Log(other.transform.root.name + ": 심장이 없음");
    //             }
    //         }
    //     }
    // }

}