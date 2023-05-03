using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class HitBoxTrigger : MonoBehaviour, IComparable<HitBoxTrigger>
{
    private Heart heart; // init
    private LayerMask targetMask; // init
    public float startTime; // 인스펙터에서 수정
    public float duration; // 인스펙터에서 수정
    [HideInInspector] public int targetCount; // 판정 타겟 수=
    public bool refreshAutomatically = false;
    [ShowIf("refreshAutomatically", true)] public float refreshInterval = 0.25f;
        
    [ShowInInspector][ReadOnly] private Damage damage; // 인스펙터에서 수정

    [BoxGroup("Skill")] [SerializeField] private float damageRatio = 1f; // 스킬 계수, 0.0 ~ 1.0
    [BoxGroup("Skill")] [SerializeField] private CC_type ccType;
    [BoxGroup("Skill")] [SerializeField] private float ccPower; // 1 ~

    private float elapsed = 0;
    private HashSet<string> hitHash = new HashSet<string>();
    private ParticleSystem particle;

    public void Init(Heart heart, LayerMask targetMask, int targetCount)
    {
        this.heart = heart;
        this.targetMask = targetMask;
        this.targetCount = targetCount;
    }

    public void Activate()
    {
        ClearHash();
        elapsed = 0;
        damage = heart.Generate_Damage(damageRatio, ccType, ccPower);
        gameObject.SetActive(true);
        if (refreshAutomatically)
        {
            RefreshHashOnTime();
        }
    }

    private void Deactivate()
    {
        // Debug.Log("deactivate");
        gameObject.SetActive(false);
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= duration)
        {
            Deactivate();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // other.gameObject.layer는 레이어 인덱스 (ex. 7)
        // targetMask는 인덱스로 시프트까지 계산된 값 (ex. 128)
        // 이상한데..
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