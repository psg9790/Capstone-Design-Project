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
    // private Heart heart;
    public LayerMask targetMask; // 타깃 레이어. 이 레이어로 설정한 오브젝트만 충돌 판정한다
    public float duration;
    public ParticleSystem parent_particle; // 발동할 파티클 시스템을 인스펙터에서 끌어놓을 것
    private PriorityQueue<HitBoxTrigger> pq = new PriorityQueue<HitBoxTrigger>();
    private HitBoxTrigger[] hitBoxTriggers;


    private Coroutine particlePlayCoroutine;

    IEnumerator ParticlePlayIE()
    {
        while (parent_particle.time < parent_particle.main.duration) // 시간이 설정한 시간만큼 play되도록 유도
        {
            yield return null;
            while (!pq.Empty() && (pq.Top().startTime >= parent_particle.time))
            {
                pq.Pop().Activate();
            }
        }

        // Particle_Stop(); // 설정한 시간을 모두 완수하면 삭제
        Destroy(this.gameObject);
    }

    public void Particle_Play(Heart heart) // 파티클 재생
    {
        transform.position = heart.gameObject.transform.position; // 가장 부모의 위치와 방향만 잡아주면 자식들은 따라감
        transform.rotation = Quaternion.LookRotation(heart.gameObject.transform.forward);

        if (heart.ATK_SPEED < 0.9999 || heart.ATK_SPEED > 1.0001)
        {
            duration = duration / heart.ATK_SPEED; // 공격속도에 따라 지속시간 변화
            ParticleSystem[] particles = parent_particle.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < particles.Length; i++)
            {
                var main = particles[i].main;
                main.simulationSpeed = heart.ATK_SPEED;
            }
        }

        hitBoxTriggers = GetComponentsInChildren<HitBoxTrigger>(true);
        for (int i = 0; i < hitBoxTriggers.Length; i++)
        {
            hitBoxTriggers[i].Init(heart, targetMask);
            hitBoxTriggers[i].startTime /= heart.ATK_SPEED;
            hitBoxTriggers[i].duration /= heart.ATK_SPEED;

            if (hitBoxTriggers[i].startTime + hitBoxTriggers[i].duration > duration) // 실수 방지를 위한 duration clamping
            {
                hitBoxTriggers[i].duration = duration - hitBoxTriggers[i].startTime;
            }

            // hitBoxTriggers[i].gameObject.SetActive(false); // 실수 방지용인데 그냥 꺼줘...
            pq.Push(hitBoxTriggers[i]);
        }

        parent_particle.Play();
        particlePlayCoroutine = StartCoroutine(ParticlePlayIE());
    }
}