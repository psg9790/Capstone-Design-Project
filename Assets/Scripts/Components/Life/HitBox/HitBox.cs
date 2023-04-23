using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public LayerMask targetMask; // 타겟레이어. 이 레이어로 설정한 오브젝트만 충돌 판정한다
    // public Collider col;
    [Required] public ParticleSystem particle; // 발동할 파티클 시스템을 인스펙터에서 끌어놓을 것

    private HashSet<string> hitHash = new HashSet<string>(); // 중복 타격을 막기 위한 hash, transform.root의 이름(string)을 사용
    [ShowInInspector] private Damage damage; // 충돌 시 상대 오브젝트에 전달한 damage


    public void Particle_Play() // 파티클 재생
    {
        ClearHash(); // 해시 초기화
        particle.Play();
    }

    public void Particle_Stop() // 파티클 정지. 시간 0으로 돌아감
    {
        particle.Stop();
    }

    // 파티클 시스템의 Collision 모듈에서 "Send Collision Message"옵션을 키고, "Type"을 Plane이 아니라 World로 설정할 것
    private void OnParticleCollision(GameObject other) 
    {
        if ((1 << other.layer) == targetMask)
        {
            if (!hitHash.Contains(other.transform.root.name)) // 최상위부모 이름을 해싱, 히트한 타겟이 해싱되어 있으면 다시 타격 x 
            {
                hitHash.Add(other.transform.root.name); // 히트한 타겟 해싱
                if (other.transform.root.TryGetComponent<Heart>(out Heart _heart))
                {
                    Vector3 dir = other.transform.position - transform.position;
                    dir.y = 0;
                    _heart.Take_Damage(damage, dir.normalized);
                }
                else
                {
                    Debug.Log(other.transform.root.name + ": 심장이 없음");
                }
            }
        }
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