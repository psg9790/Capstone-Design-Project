// Player나 Monster에 별도 컴포넌트로 부착

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Monsters.Skill;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[System.Serializable]
public class Heart : MonoBehaviour
{
    [FoldoutGroup("Events")] public UnityEvent<float, Vector3> OnHit; // 색상 전환용 이벤트 // <duration>
    [FoldoutGroup("Events")] public UnityEvent<Vector3> OnStiff; // cc기를 이벤트로 처리해서 상태 전이 // <duration>
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
    public bool immune; // 데미지, cc 모두 면역

    [FoldoutGroup("Attributes")] [SerializeField]
    public bool cc_stiff_immune; // 경직 면역

    [FoldoutGroup("Attributes")] [SerializeField]
    public bool cc_knockback_immune; // 넉백 면역

    
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

    public void ATK_SPEED_CHANGE(int n)
    {
        atk_speed += n;
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
        bool isCrit = (Random.Range(0f, 100f) < CRITICAL_RATE) ? true : false;
        float calDamage = (ATK * dmgRate) * (isCrit ? (CRITICAL_DAMAGE * 0.01f) : 1);
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
        if (immune)
            return;
        // 내부 처리
        float damage = dmg.damage;
        damage *= 250 / (250 + DEF); // 플레이어 방어력 최대 35*6=210 (경감률=250/(250+210)=45.6xx%)
        damage = (int)(damage * Random.Range(0.75f, 1f));
        
        cur_hp = Mathf.Clamp(cur_hp - damage, 0, MAX_HP);
        
        OnHit.Invoke(0.5f, dir);
        if (useDamageFont)
        {
            DamageFontManager.Instance.GenerateDamageFont(transform.position + Vector3.up * 0.5f, damage, dmg.isCritical, damageFont_randomRange);
        }

        if (!cc_stiff_immune && // 경직 저항있으면 무시
            dmg.ccType == CC_type.Stiff)
        {
            OnStiff.Invoke(dir);
        }

        if (!cc_knockback_immune && // 넉백 저항있으면 무시
            dmg.ccType == CC_type.Knockback)
        {
            OnKnockBack.Invoke(dmg.ccPower, dir);
        }

        if (cur_hp <= 0)
        {
            OnDeath.Invoke();
        }
    }


    private StringBuilder sb = new StringBuilder();
    [Button]
    public void PlayerItemEquip()
    {
        if (gameObject.layer != LayerMask.NameToLayer("Player"))
            return;
        
        if (Inventory.instance == null)
        {
            Debug.LogError("Inventory 인스턴스가 없습니다");
            return;
        }
        
        float calcATK = 20;
        float calcDEF = 5;
        float calcHP = 100;
        float calcATKSPEED = 1;
        float calcMOVEMENTSPEED = 5;
        float calcCRITRATE = 5f;
        float calcCRITDAMAGE = 200;

        float ATKbyArtifact = 0,
            DEFbyArtifact = 0,
            HPbyArtifact = 0,
            ATKSPEEDbyArtifact = 0,
            MOVEMENTSPEEDbyArtifact = 0,
            CRITRATEbyArtifact = 0;
        float ATKbyWeapon = 0,
            ATKSPEEDbyWeapon = 0,
            CRITRATEbyWeapon = 0,
            CRITDAMAGEbyWeapon = 0;
        

        if (Inventory.instance.tempItem == null)
        {
            // 기본 스탯
            atk = calcATK;
            def = calcDEF;
            max_hp = calcHP;
            atk_speed = calcATKSPEED;
            movement_speed = calcMOVEMENTSPEED;
            Player.Instance.nav.speed = movement_speed;
            criticalRate = calcCRITRATE;
            criticalDamage = calcCRITDAMAGE;
        }
        
        for (int i = 0; i < Inventory.instance.artifactUIs.Length; i++)
        {
            if (Inventory.instance.artifactUIs[i].isInstallation)
            {
                if ((Inventory.instance.artifactUIs[i].itemSlot.itemSlotui.item as Artifact).options.ContainsKey(ArtifactKey.ATK))
                {
                    ATKbyArtifact += (Inventory.instance.artifactUIs[i].itemSlot.itemSlotui.item as Artifact)
                        .options[ArtifactKey.ATK];
                }
                if ((Inventory.instance.artifactUIs[i].itemSlot.itemSlotui.item as Artifact).options.ContainsKey(ArtifactKey.DEF))
                {
                    DEFbyArtifact += (Inventory.instance.artifactUIs[i].itemSlot.itemSlotui.item as Artifact)
                        .options[ArtifactKey.DEF];
                }
                if ((Inventory.instance.artifactUIs[i].itemSlot.itemSlotui.item as Artifact).options.ContainsKey(ArtifactKey.HP))
                {
                    HPbyArtifact += (Inventory.instance.artifactUIs[i].itemSlot.itemSlotui.item as Artifact)
                        .options[ArtifactKey.HP];
                }
                if ((Inventory.instance.artifactUIs[i].itemSlot.itemSlotui.item as Artifact).options.ContainsKey(ArtifactKey.ATKSPEED))
                {
                    ATKSPEEDbyArtifact += (Inventory.instance.artifactUIs[i].itemSlot.itemSlotui.item as Artifact)
                        .options[ArtifactKey.ATKSPEED];
                }
                if ((Inventory.instance.artifactUIs[i].itemSlot.itemSlotui.item as Artifact).options.ContainsKey(ArtifactKey.MOVEMENTSPEED))
                {
                    MOVEMENTSPEEDbyArtifact += (Inventory.instance.artifactUIs[i].itemSlot.itemSlotui.item as Artifact)
                        .options[ArtifactKey.MOVEMENTSPEED];
                }
                if ((Inventory.instance.artifactUIs[i].itemSlot.itemSlotui.item as Artifact).options.ContainsKey(ArtifactKey.CRIT_RATE))
                {
                    CRITRATEbyArtifact += (Inventory.instance.artifactUIs[i].itemSlot.itemSlotui.item as Artifact)
                        .options[ArtifactKey.CRIT_RATE];
                }
            }
        }

        if (Inventory.instance.tempItem != null)
        {
            ATKbyWeapon = (Inventory.instance.tempItem as Weapon).options[WeaponKey.ATK];
            ATKSPEEDbyWeapon = (Inventory.instance.tempItem as Weapon).options[WeaponKey.ATKSPEED];
            CRITRATEbyWeapon = (Inventory.instance.tempItem as Weapon).options[WeaponKey.CRIT_RATE];
            CRITDAMAGEbyWeapon = (Inventory.instance.tempItem as Weapon).options[WeaponKey.CRIT_DAMAGE];
        }

        calcATK = (float)Math.Round((calcATK + ATKbyArtifact) * (1 + ATKbyWeapon * 0.01f), 2);
        calcDEF = (float)Math.Round(calcDEF + DEFbyArtifact, 2);
        calcHP = (float)Math.Round(calcHP + HPbyArtifact, 2);
        calcATKSPEED = (float)Math.Round(calcATKSPEED + ATKSPEEDbyArtifact + ATKSPEEDbyWeapon, 2);
        calcMOVEMENTSPEED = (float)Math.Round(calcMOVEMENTSPEED + MOVEMENTSPEEDbyArtifact, 2);
        calcCRITRATE = (float)Math.Round(calcCRITRATE + CRITRATEbyWeapon + CRITRATEbyArtifact, 2);
        calcCRITDAMAGE = (float)Math.Round(calcCRITDAMAGE + CRITDAMAGEbyWeapon, 2);

        // 공격력 계산 : (기본 공격력 + 아티팩트 공격력) * 무기 공격력 %
        atk = calcATK;
        
        // 방어력 계산 : (기본 방어력 + 아티팩트 방어력)
        def = calcDEF;
        
        // 체력 계산 : (기본 체력 + 아티팩트 체력)
        max_hp = calcHP;
        
        // 공격속도 계산 : (기본 공격속도 + 아티팩트 공격속도)
        atk_speed = calcATKSPEED;
        
        // 이동속도 계산 : (기본 이동속도 + 아티팩트 이동속도) 최대이속 23
        movement_speed = calcMOVEMENTSPEED;
        Player.Instance.nav.speed = movement_speed;

        // 치명확률 계산
        criticalRate = calcCRITRATE;
        
        // 치명데미지 계산
        criticalDamage = calcCRITDAMAGE;
        
        sb.Clear();
        sb.Append("공격력: ");
        sb.Append(atk.ToString());
        sb.Append("\n");
        sb.Append("방어력: ");
        sb.Append(def.ToString());
        sb.Append("\n");
        sb.Append("체력: ");
        sb.Append(max_hp.ToString());
        sb.Append("\n");
        sb.Append("공격속도: ");
        sb.Append(atk_speed.ToString());
        sb.Append("\n");
        sb.Append("이동속도: ");
        sb.Append(movement_speed.ToString());
        sb.Append("\n");
        sb.Append("치명타 확률: ");
        sb.Append(criticalRate.ToString());
        sb.Append("%");
        sb.Append("\n");
        sb.Append("치명타 피해: ");
        sb.Append(criticalDamage.ToString());
        sb.Append("%");
        sb.Append("\n");
        sb.Append("\n");
        sb.Append("무기");
        sb.Append("\n");
        sb.Append("공격력 +");
        sb.Append(ATKbyWeapon.ToString());
        sb.Append("%");
        sb.Append("\n");
        sb.Append("공격속도 +");
        sb.Append(ATKSPEEDbyWeapon.ToString());
        sb.Append("\n");
        sb.Append("치명타 피해 +");
        sb.Append(CRITDAMAGEbyWeapon.ToString());
        sb.Append("%");
        sb.Append("\n");
        sb.Append("\n");
        sb.Append("아티팩트");
        sb.Append("\n");
        sb.Append("공격력 +");
        sb.Append(ATKbyArtifact.ToString());
        sb.Append("\n");
        sb.Append("방어력 +");
        sb.Append(DEFbyArtifact.ToString());
        sb.Append("\n");
        sb.Append("체력 +");
        sb.Append(HPbyArtifact.ToString());
        sb.Append("\n");
        sb.Append("공격속도 +");
        sb.Append(ATKSPEEDbyArtifact.ToString());
        sb.Append("\n");
        sb.Append("이동속도 +");
        sb.Append(MOVEMENTSPEEDbyArtifact.ToString());
        sb.Append("\n");
        sb.Append("치명타 확률 +");
        sb.Append(CRITRATEbyArtifact.ToString());
        sb.Append("%");
        sb.Append("\n");
        Inventory.instance.SetSideStatDisplayText(sb.ToString());

    }

    public void SetMonsterStatByLevel(short level) // 이거 쓸것 (매개변수 월드레벨)
    {
        this.level = level;
        GetComponent<SkillSet>().SetMonsterStatByLevel(level);
    }

    public void SetStat(float atk, float hp, float def, float atkspeed, float movespeed)
    {
        this.atk = atk;
        this.max_hp = hp;
        cur_hp = max_hp;
        this.def = def;
        this.atk_speed = atkspeed;
        this.movement_speed = movespeed;
        GetComponent<NavMeshAgent>().speed = movement_speed;
    }
}