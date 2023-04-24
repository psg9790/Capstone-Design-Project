// <노트>
// 이 스크립트를 파티클 시스템이 있는 오브젝트에 부착
// 해당 파티클 시스템의 collision 모듈을 키고, type을 world로 변경
// ** 파티클 시스템을 프리팹에서 꺼놓을 필요 없음 ** Awake 함수에서 파티클 시스템 찾고 타깃 레이어 설정하므로 오류남

using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = System.Object;

public class HitBox : MonoBehaviour
{
    private Heart heart;
    public LayerMask targetMask; // 타깃 레이어. 이 레이어로 설정한 오브젝트만 충돌 판정한다
    public float duration;
    private ParticleSystem particle; // 발동할 파티클 시스템을 인스펙터에서 끌어놓을 것
    private PriorityQueue<HitBoxTrigger> pq = new PriorityQueue<HitBoxTrigger>();
    private HitBoxTrigger[] hitBoxTriggers;

    // private HashSet<string> hitHash = new HashSet<string>(); // 중복 타격을 막기 위한 hash, transform.root의 이름(string)을 사용
    // [ShowInInspector] private Damage damage; // 충돌 시 상대 오브젝트에 전달한 damage
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        heart = GetComponentInParent<Heart>();
        hitBoxTriggers = GetComponentsInChildren<HitBoxTrigger>(true);
        for (int i = 0; i < hitBoxTriggers.Length; i++)
        {
            hitBoxTriggers[i].Init(heart, targetMask);
            if (hitBoxTriggers[i].startTime + hitBoxTriggers[i].duration > duration)
            {
                hitBoxTriggers[i].duration = duration - hitBoxTriggers[i].startTime;
            }
            hitBoxTriggers[i].gameObject.SetActive(false);
        }

        // // https://docs.unity3d.com/2021.3/Documentation/ScriptReference/ParticleSystem.CollisionModule-collidesWith.html
        // var collision = particle.collision;
        // collision.collidesWith = targetMask;
    }

    private Coroutine particlePlayCoroutine;
    IEnumerator ParticlePlayIE()
    {
        while (particle.time < particle.duration)
        {
            yield return null;
            while (!pq.Empty() && (pq.Top().startTime >= particle.time))
            {
                pq.Pop().Activate();
            }
        }
        Particle_Stop();
    }
    
    public void Particle_Play() // 파티클 재생
    {
        // ClearHash(); // 해시 초기화
        pq.Clear();
        for (int i = 0; i < hitBoxTriggers.Length; i++)
        {
            pq.Push(hitBoxTriggers[i]);
        }
        particle.Play();
        particlePlayCoroutine = StartCoroutine(ParticlePlayIE());
    }


    public void Particle_Stop() // 파티클 정지. 시간 0으로 돌아감
    {
        particle.Stop();
    }
    
    // private void OnTriggerEnter(Collider other)
    // {
    //     // other.gameObject.layer는 레이어 인덱스 (ex. 7)
    //     // targetMask는 인덱스로 시프트까지 계산된 값 (ex. 128)
    //     // 이상한데..
    //     if ((1 << other.gameObject.layer) == targetMask)
    //     {
    //         if (!hitHash.Contains(other.transform.root.name)) // 최상위부모 이름,,, 히트한 타겟이 해싱되어 있으면 다시 타격 x 
    //         {
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
    //
    // public void SetDamage(Damage _damage)
    // {
    //     damage = _damage;
    // }
    //
    // [Button]
    // public void ClearHash()
    // {
    //     hitHash.Clear();
    // }
    //
    // private void OnParticleSystemStopped()
    // {
    //     Debug.Log("end");
    // }

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
}