// <노트>
// 이 스크립트를 파티클 시스템이 있는 오브젝트에 부착
// 해당 파티클 시스템의 collision 모듈을 키고, type을 world로 변경
// ** 파티클 시스템을 프리팹에서 꺼놓을 필요 없음 ** Awake 함수에서 파티클 시스템 찾고 타깃 레이어 설정하므로 오류남

using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

public class HitBox : MonoBehaviour
{
    public LayerMask targetMask; // 타깃 레이어. 이 레이어로 설정한 오브젝트만 충돌 판정한다
    [HideInInspector] public LayerMask heartLayer;
    [ReadOnly] public float elapsed = 0; // 경과 시간
    public float duration; // 총 플레이 시간
    public int targetCount = 5; // 최대 때릴 수 있는 마릿수
    public ParticleSystem parent_particle; // 발동할 파티클 시스템을 인스펙터에서 끌어놓을 것
    private PriorityQueue<HitBoxTrigger> pq = new PriorityQueue<HitBoxTrigger>();
    private HitBoxTrigger[] hitBoxTriggers;

    public bool isBullet = false;
    [ShowIf("isBullet")] public float bulletSpeed = 20f;
    [InfoBox("방향을 수정하고 싶으면 이펙트 생성 후, Particle_Play 함수 호출 직전에 코드로 \"bulletDirection\"변수를 " +
             "원하는 방향으로 초기화 해줄 것 (초기화 하지 않으면 기본은 정면)")] 
    [ShowIf("isBullet")] [ReadOnly] public Vector3 bulletDirection = Vector3.zero; // bullet의 방향
    [ShowIf("isBullet")] public ParticleSystem bulletHitEffect;
    [ShowIf("isBullet")] public bool isHoming = false;
    [ShowIf("isHoming")] [CustomValueDrawer("MyCustomDrawerStatic")] public float homingPerformance = 0.1f;

    private static float MyCustomDrawerStatic(float value, GUIContent label)
    {
        return EditorGUILayout.Slider(label, value, 0f, 1f);
    }

    private Coroutine particlePlayCoroutine;
    private Coroutine bulletPlayCoroutine;

    IEnumerator ParticlePlayIE()
    {
        while (elapsed < duration) // 시간이 설정한 시간만큼 play되도록 유도
        {
            elapsed += Time.deltaTime;
            yield return null;
            while (!pq.Empty() && (pq.Top().startTime <= elapsed))
            {
                pq.Pop().Activate();
            }
        }
        
        // 설정한 시간을 모두 완수하면 삭제
        Destroy(this.gameObject);
    }

    IEnumerator BulletPlayIE()
    {
        while (elapsed < duration) // 시간이 설정한 시간만큼 play되도록 유도
        {
            elapsed += Time.deltaTime;
            if (isHoming)
            {
                int layer = LayerMask.NameToLayer("Player");
                if (Mathf.Log(targetMask, 2) == layer)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation,
                        Quaternion.LookRotation(Player.Instance.transform.position - transform.position), homingPerformance);
                }
            }
            transform.position = transform.position + transform.forward * bulletSpeed * Time.deltaTime;
            yield return null;
            while (!pq.Empty() && (pq.Top().startTime <= elapsed))
            {
                pq.Pop().Activate();
            }
        }
        
        // 설정한 시간을 모두 완수하면 삭제
        Destroy(this.gameObject);
    }

    public void BulletHit_Play(Vector3 hitPoint, Vector3 dir)
    {
        StopCoroutine(bulletPlayCoroutine);
        // hit 이펙트 생성
        if (bulletHitEffect != null)
        {
            // -dir 사용
            ParticleSystem bh = Instantiate(bulletHitEffect);
            bh.transform.position = hitPoint;
            bh.transform.LookAt(hitPoint - dir);
        }
        Destroy(this.gameObject);
    }

    public void Particle_Play(Heart heart) // 파티클 재생
    {
        heartLayer = heart.gameObject.layer;
        
        transform.position = heart.gameObject.transform.position; // 가장 부모의 위치와 방향만 잡아주면 자식들은 따라감
        transform.rotation = Quaternion.LookRotation(heart.gameObject.transform.forward);
        if (isBullet)
        {
            if (bulletDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(bulletDirection);
            }
        }

        if (heart.ATK_SPEED < 0.9999 || heart.ATK_SPEED > 1.0001) // 배속설정 되어있으면 적용
        {
            duration = duration / heart.ATK_SPEED; // 공격속도에 따라 지속시간 변화
            ParticleSystem[] particles = parent_particle.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < particles.Length; i++)
            {
                var main = particles[i].main;
                main.simulationSpeed = heart.ATK_SPEED;
            }
        }

        hitBoxTriggers = GetComponentsInChildren<HitBoxTrigger>(true); // hitboxtrigger 우선순위큐 초기화
        for (int i = 0; i < hitBoxTriggers.Length; i++)
        {
            hitBoxTriggers[i].gameObject.SetActive(false);
            hitBoxTriggers[i].Init(heart, targetMask, targetCount, this);
            hitBoxTriggers[i].startTime /= heart.ATK_SPEED;
            hitBoxTriggers[i].duration /= heart.ATK_SPEED;

            if (hitBoxTriggers[i].startTime + hitBoxTriggers[i].duration > duration) // 실수 방지를 위한 duration clamping
                hitBoxTriggers[i].duration = duration - hitBoxTriggers[i].startTime;
            
            pq.Push(hitBoxTriggers[i]);
        }

        parent_particle.Play();
        if (isBullet)
            bulletPlayCoroutine = StartCoroutine(BulletPlayIE());
        else
            particlePlayCoroutine = StartCoroutine(ParticlePlayIE());
    }
}