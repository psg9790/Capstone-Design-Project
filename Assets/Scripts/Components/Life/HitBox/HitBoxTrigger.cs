using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class HitBoxTrigger : MonoBehaviour, IComparable<HitBoxTrigger>
{
    private Heart heart; // 데미지 생성용
    private LayerMask targetMask; // 타겟 layer
    public float startTime; // 이 판정이 켜질 타이밍 (0 ~ )
    private float elapsed = 0; // 실행 시간 카운트용
    public float duration; // 이 판정 지속시간
    [HideInInspector] public int targetCount; // 판정이 타격할 타겟 수
    public bool refreshAutomatically = false; // 이 트리거를 반복 재생 할 것인가? (공격판정을 주기적으로 초기화 할 것인가?)
    [ShowIf("refreshAutomatically", true)] public float refreshInterval = 0.25f; // 반복재생할 시간 간격

    [ShowInInspector] [ReadOnly] private Damage damage; // 인스펙터에서 수정

    [BoxGroup("Skill")] [SerializeField] private float damageRatio = 1f; // 스킬 계수, 0.0 ~ 1.0
    [BoxGroup("Skill")] [SerializeField] private CC_type ccType; // cc기
    [BoxGroup("Skill")] [SerializeField] private float ccPower; // 1 ~


    private HashSet<string> hitHash = new HashSet<string>(); // 타격 대상 중복타격 방지 위해 set 사용

    private HitBox hitBox; // bullet 플래그 확인용 저장

    public void Init(Heart heart, LayerMask targetMask, int targetCount, HitBox hitBox) // 초기 설정
    {
        this.heart = heart;
        this.targetMask = targetMask;
        this.targetCount = targetCount;
        this.hitBox = hitBox;
        if (TryGetComponent<Rigidbody>(out Rigidbody rigid))
        {
            rigid.useGravity = false;
            rigid.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
        else
        {
            Rigidbody rig = this.gameObject.AddComponent<Rigidbody>();
            rig.useGravity = false;
            rig.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    public void Activate() // 실행
    {
        elapsed = 0;
        damage = heart.Generate_Damage(damageRatio, ccType, ccPower);
        gameObject.SetActive(true);

        if (refreshAutomatically)
        {
            RefreshHashOnTime();
        }

        if (!hitBox.isBullet) // bullet의 생명주기는 HitBox 스크립트에서 관리
        {
            DOVirtual.DelayedCall(duration, () => Deactivate());
        }
    }

    private void Deactivate() // 특정 조건 채울 시 (타겟 최대 수 초과, 실행 시간 종료 등) 판정 강제 종료
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        // other.gameObject.layer는 레이어 인덱스 (ex. 7)
        // targetMask는 인덱스로 시프트까지 계산된 값 (ex. 128)

        
        int targetLayer = (int)Mathf.Log(targetMask, 2);
        if (hitBox.isBullet) // bullet일 시 분기
        {
            Vector3 dir = other.transform.position - heart.transform.position;
            dir.y = 0;
            dir = dir.normalized;

            if (other.gameObject.layer == targetLayer)
            {
                if (other.TryGetComponent<Heart>(out Heart _heart))
                {
                    if (hitHash.Contains(_heart.transform.gameObject.name))
                        return;
                    hitHash.Add(_heart.transform.gameObject.name);
                    _heart.Take_Damage(damage, dir.normalized);
                    if (Physics.Raycast(transform.position, dir, out RaycastHit hit, Mathf.Infinity, 1 << targetLayer))
                    {
                        dir.y = 0;
                        hitBox.BulletHit(hit.point, dir);
                    }
                    else
                    {
                        hitBox.BulletHit(transform.position, Vector3.zero);
                    }

                    // if (hitHash.Count >= targetCount) // 혀용 타수 초과시 파괴
                    //     Destroy(hitBox.gameObject);
                }
            }
            else if (other.gameObject.layer != hitBox.heartLayer) // 일반 오브젝트 (ex.벽) 부딪히면 파괴
            {
                // UnityEngine.Debug.Log(LayerMask.LayerToName(other.gameObject.layer));
                // UnityEngine.Debug.Log(other.gameObject.name);
                // if (Physics.Raycast(transform.position, dir, out RaycastHit hit, Mathf.Infinity,
                //         1 << other.gameObject.layer))
                // {
                //     dir.y = 0;
                //     hitBox.BulletHit(hit.point, dir);
                // }
                // else
                {
                    hitBox.BulletHit(transform.position, Vector3.zero);
                }
                Destroy(hitBox.gameObject);
            }

            return;
        }

        // 일반 판정
        if (other.gameObject.layer == targetLayer)
        {
            if (!hitHash.Contains(other.transform.name)) // 최상위부모 이름,,, 히트한 타겟이 해싱되어 있으면 다시 타격 x 
            {
                if (other.transform.TryGetComponent<Heart>(out Heart _heart))
                {
                    hitHash.Add(other.transform.name); // 히트한 타겟 해싱
                    Vector3 dir = other.transform.position - transform.position;
                    dir.y = 0;
                    _heart.Take_Damage(damage, dir.normalized);
                    hitBox.SlashHit();
                    

                    if (hitHash.Count >= targetCount)
                    {
                        Deactivate();
                    }
                }
                else
                {
                    Debug.Log(other.transform.name + ": 심장이 없음");
                }
            }
        }
    }

    [Button]
    public void ClearHash() // 해시 초기화
    {
        hitHash.Clear();
    }

    public void RefreshHashOnTime() // 주기적으로 해시 초기화하는 코루틴 실행하는 함수
    {
        StartCoroutine(ClearHashOnSecondsCo(refreshInterval));
    }

    IEnumerator ClearHashOnSecondsCo(float phase) // 설정해놓은 주기로 해시를 초기화하는 코루틴
    {
        while (elapsed <= duration)
        {
            yield return new WaitForSeconds(phase);
            ClearHash();
        }
    }

    public int CompareTo(HitBoxTrigger other) // 우선순위큐 정렬기준이 되는 비교연산자
    {
        // id 멤버를 기준으로 크기를 비교
        if (startTime == other.startTime)
            return 0;
        return startTime < other.startTime ? 1 : -1;
    }
}