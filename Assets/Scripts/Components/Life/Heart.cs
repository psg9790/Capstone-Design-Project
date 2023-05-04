// Player나 Monster에 별도 컴포넌트로 부착

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[System.Serializable]
public class Heart : MonoBehaviour
{
    [FoldoutGroup("Events")] public UnityEvent<float, Vector3> OnHit; // 색상 전환용 이벤트 // <duration>
    [FoldoutGroup("Events")] public UnityEvent OnStiff; // cc기를 이벤트로 처리해서 상태 전이 // <duration>
    [FoldoutGroup("Events")] public UnityEvent OnDeath; // 죽을 때 실행될 이벤트, 각 객체에서 알맞는 죽는 처리를 listener에 추가할 것
    [FoldoutGroup("Events")] public UnityEvent<float, Vector3> OnKnockBack; // 넉백 시 발생할 이벤트

    [FoldoutGroup("Attributes")]
    [InfoBox("이 변수들을 플레이 중 직접 수정하면 다른 오브젝트 작동 시 (장비 장착/해제 등) 오류가 발생할 수도 있습니다.")]
    [SerializeField]
    private short level; // 레벨이 속값으로 존재해야 될 것 같은게 이 값에 따라 드랍하는 아이템의 수치를 정해줘야 할듯

    [FoldoutGroup("Attributes")] [SerializeField]
    private float max_hp; // 최대 체력

    [FoldoutGroup("Attributes")] [SerializeField]
    private float cur_hp; // 현재 체력

    [FoldoutGroup("Attributes")] [SerializeField]
    private float atk; // 현재 공격력

    [FoldoutGroup("Attributes")] [SerializeField]
    private float def; // 현재 방어력

    [FoldoutGroup("Attributes")] [SerializeField]
    private float movement_speed = 1;

    [FoldoutGroup("Attributes")] [SerializeField]
    private float atk_speed = 1;

    [FoldoutGroup("Attributes")] [SerializeField]
    private float skill_cooldown;

    [FoldoutGroup("Attributes")] [SerializeField]
    private float criticalRate; // 0 ~ 100

    [FoldoutGroup("Attributes")] [SerializeField]
    private float criticalDamage = 2f; // *1, *1.1, *1.5...
    
    [FoldoutGroup("Attributes")] [SerializeField]
    private bool immune; // 데미지, cc 모두 면역

    [FoldoutGroup("Attributes")] [SerializeField]
    private bool cc_stiff_immune; // 경직 면역

    [FoldoutGroup("Attributes")] [SerializeField]
    private bool cc_knockback_immune; // 넉백 면역

    
    public short LEVEL => level;
    public float MAX_HP => max_hp;
    public float CUR_HP => cur_hp;
    public float ATK => atk;
    public float DEF => def;
    public float MOVEMENT_SPEED => movement_speed;
    public float ATK_SPEED => atk_speed;
    public float SKILL_COOLDOWN => skill_cooldown;
    public float CRITICAL_RATE => criticalRate;
    public float CRITICAL_DAMAGE => criticalDamage;

    public bool useMonsterHpBar = true; // 잡몹용 hpbar ui를 사용할거면 true
    [ShowIf("useMonsterHpBar")][ReadOnly] public HPbar_custom hpbar; // hpbar 오브젝트
    [Required] public Transform upper_pos; // 이 몬스터의 hpbar가 달려야 할 위치 (3D->2D)
    public bool useDamageFont = true; // 데미지 폰트 사용여부 (인스펙터에서 수정)
    [ShowIf("useDamageFont", true)] public Vector3 damageFont_randomRange = new Vector3(-50, 50, 100); // -x, +x, +y

    private void Awake()
    {
        if (HPbarManager.Instance == null)
        {
            GameObject cvs = new GameObject("HpbarManager + canvas"); // Hpbar 매니저가 없으면 생성함
            cvs.AddComponent<HPbarManager>().Init();
        }
        if (DamageFontManager.Instance == null) // 데미지 폰트 띄우는 매니저 생성
        {
            GameObject dmg = new GameObject("DamageFontManager + canvas"); // DamageFont 매니저가 없으면 생성함
            dmg.AddComponent<DamageFontManager>().Init();
        }
        if (useMonsterHpBar) // 일반 몬스터인 경우 hpbar UI 생성, 생성과 함께 pool이 존재하지 않으면 생성해서 소속됨
        {
            // hpbar = Instantiate(Resources.Load("UI/hpbar")).GetComponent<HPbar_custom>();
            hpbar = Instantiate(HPbarManager.Instance.hpbar).GetComponent<HPbar_custom>(); // 이 객체가 HPbar를 사용하면 오브젝트를 생성
            hpbar.Activate(this);
        }
    }

    private void OnDestroy()
    {
        if (useMonsterHpBar)
        {
            if (hpbar != null) // 링크되어 있는 hpbar 오브젝트도 함께 삭제
            {
                Destroy(hpbar.gameObject);
            }
        }
    }

    public void Restore_CUR_HP(float amount)
    {
        cur_hp += amount;
        if (cur_hp > max_hp)
        {
            cur_hp = max_hp;
        }
    }

    [FoldoutGroup("Functions")]
    [Button]
    public void RestoreAll_CUR_HP()
    {
        cur_hp = max_hp;
    }

    [FoldoutGroup("Functions")]
    [Button]
    public void ForceDead() // 강제 kill
    {
        cur_hp = 0;
        OnDeath.Invoke();
    }

    public Damage Generate_Damage(float dmgRate, CC_type cc, float power) // 외부에서 이 heart 기반으로 데미지 추출할 때 사용
    {
        float rand = Random.Range(0f, 100f);
        bool isCrit = (rand < CRITICAL_RATE) ? true : false;
        float calDamage = (ATK * dmgRate) * (isCrit ? CRITICAL_DAMAGE : 1);
        Damage dmg = new Damage(calDamage, isCrit);
        if (cc != CC_type.None)
        {
            dmg.ccType = cc;
            dmg.ccPower = power;
        }

        return dmg;
    }
    
    [FoldoutGroup("Functions")]
    [Button]
    public void Take_Damage(Damage dmg, Vector3 dir) // 데미지 피해 입음
    {
        // 내부 처리
        float ins = dmg.damage;
        ins = (int)(ins * Random.Range(0.75f, 1f));
        
        cur_hp -= ins;
        OnHit.Invoke(0.5f, -dir);
        if (useDamageFont)
        {
            DamageFontManager.Instance.GenerateDamageFont(transform.position + Vector3.up * 0.5f, ins, dmg.isCritical, damageFont_randomRange);
        }

        if (!cc_stiff_immune && // 경직 저항있으면 무시
            dmg.ccType == CC_type.Stiff)
        {
            OnStiff.Invoke();
        }

        if (!cc_knockback_immune && // 넉백 저항있으면 무시
            dmg.ccType == CC_type.Knockback)
        {
            OnKnockBack.Invoke(dmg.ccPower, dir);
        }

        if (cur_hp <= 0)
        {
            cur_hp = 0;
            OnDeath.Invoke();
        }
    }


    [FoldoutGroup("Functions")]
    [Button]
    public void Take_Damage_DOT(Damage _damage, Vector3 dir, float _tik, float _time)
    {
        // 초 처리 

        if (cur_hp <= 0)
        {
            Debug.Log("dead");
            OnDeath.Invoke();
        }
    }
}