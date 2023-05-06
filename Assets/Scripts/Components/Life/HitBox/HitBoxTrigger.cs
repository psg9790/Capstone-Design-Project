using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class HitBoxTrigger : MonoBehaviour, IComparable<HitBoxTrigger>
{
    private Heart heart; // init
    private LayerMask targetMask; // init
    public float startTime; // 인스펙터에서 수정
    public float duration; // 인스펙터에서 수정
    [HideInInspector] public int targetCount; // 판정 타겟 수=
    public bool refreshAutomatically = false; // 이 트리거를 반복 재생 할 것인가? (공격판정을 주기적으로 초기화 할 것인가?)
    [ShowIf("refreshAutomatically", true)] public float refreshInterval = 0.25f; // 반복재생할 시간 간격

    [ShowInInspector] [ReadOnly] private Damage damage; // 인스펙터에서 수정

    [BoxGroup("Skill")] [SerializeField] private float damageRatio = 1f; // 스킬 계수, 0.0 ~ 1.0
    [BoxGroup("Skill")] [SerializeField] private CC_type ccType; // cc기
    [BoxGroup("Skill")] [SerializeField] private float ccPower; // 1 ~

    private float elapsed = 0; // 실행 시간 카운트용

    private HashSet<string> hitHash = new HashSet<string>(); // 타격 대상 중복타격 방지 위해 set 사용

    // private ParticleSystem particle;
    private HitBox hitBox;

    public void Init(Heart heart, LayerMask targetMask, int targetCount, HitBox hitBox) // 초기 설정
    {
        this.heart = heart;
        this.targetMask = targetMask;
        this.targetCount = targetCount;
        this.hitBox = hitBox;
    }

    public void Activate() // 실행
    {
        // ClearHash();
        elapsed = 0;
        damage = heart.Generate_Damage(damageRatio, ccType, ccPower);
        gameObject.SetActive(true);
        if (refreshAutomatically)
        {
            RefreshHashOnTime();
        }

        DOVirtual.DelayedCall(duration, () => Deactivate());
    }

    private void Deactivate() // 특정 조건 채울 시 (타겟 최대 수 초과, 실행 시간 종료 등) 판정 강제 종료
    {
        // Debug.Log("deactivate");
        gameObject.SetActive(false);
    }
    //
    // private void Update()
    // {
    //     elapsed += Time.deltaTime;
    //     if (elapsed >= duration)
    //     {
    //         Deactivate();
    //     }
    // }

    private void OnTriggerStay(Collider other)
    {
        // other.gameObject.layer는 레이어 인덱스 (ex. 7)
        // targetMask는 인덱스로 시프트까지 계산된 값 (ex. 128)


        if (hitBox.isBullet) // bullet일 시 분기
        {
            Vector3 dir = other.transform.position - transform.position;
            int targetLayer = (int)Mathf.Log(targetMask, 2);

            if (other.gameObject.layer == targetLayer)
            {
                if (other.TryGetComponent<Heart>(out Heart _heart))
                {
                    Physics.Raycast(transform.position, dir, out RaycastHit hit, Mathf.Infinity, 1 << targetLayer);
                    // Debug.DrawRay(transform.position, dir, Color.red, 10f);
                    _heart.Take_Damage(damage, dir.normalized);
                    Deactivate();
                    dir.y = 0;
                    hitBox.BulletHit_Play(hit.point, dir);
                }
            }
            else if (other.gameObject.layer != hitBox.heartLayer)
            {
                Physics.Raycast(transform.position, dir, out RaycastHit hit, Mathf.Infinity,
                    1 << other.gameObject.layer);
                Deactivate();
                dir.y = 0;
                hitBox.BulletHit_Play(hit.point, dir);
            }

            return;
        }

        if ((1 << other.gameObject.layer) == targetMask)
        {
            if (!hitHash.Contains(other.transform.root.name)) // 최상위부모 이름,,, 히트한 타겟이 해싱되어 있으면 다시 타격 x 
            {
                hitHash.Add(other.transform.root.name); // 히트한 타겟 해싱
                if (other.transform.root.TryGetComponent<Heart>(out Heart _heart))
                {
                    // Debug.Log(_heart.gameObject.name);
                    Vector3 dir = other.transform.position - transform.position;
                    dir.y = 0;
                    _heart.Take_Damage(damage, dir.normalized);

                    if (hitHash.Count >= targetCount)
                    {
                        Deactivate();
                    }
                }
                else
                {
                    Debug.Log(other.transform.root.name + ": 심장이 없음");
                }
            }
        }
    }

    [Button]
    public void ClearHash()
    {
        hitHash.Clear();
    }

    [Button]
    public void DebugActivate()
    {
        elapsed = 0;
        duration = 999999999;
        gameObject.SetActive(true);
    }

    public void RefreshHashOnTime()
    {
        StartCoroutine(ClearHashOnSecondsCo(refreshInterval));
    }

    IEnumerator ClearHashOnSecondsCo(float phase)
    {
        while (elapsed <= duration)
        {
            yield return new WaitForSeconds(phase);
            // Debug.Log("clear hash " + elapsed.ToString());
            ClearHash();
        }
    }

    public int CompareTo(HitBoxTrigger other)
    {
        // id 멤버를 기준으로 크기를 비교
        if (startTime == other.startTime)
            return 0;
        return startTime < other.startTime ? 1 : -1;
    }
}