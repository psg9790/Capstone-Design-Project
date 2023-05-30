using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class RecordLevelManager : MonoBehaviour
{
    private static RecordLevelManager instance;
    public static RecordLevelManager Instance => instance;
    
    public GameObject playerPrefab;
    public Transform bossMap;
    public CameraController camController;
    [Required] public NextLevelPortal nextLevelPortal_prefab;


    [ReadOnly] public int curLevel = 0;
    private Monsters.Monster[] bossPrefabs;
    private Monsters.Monster boss;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            bossPrefabs = Resources.LoadAll<Monsters.Monster>("Monsters/Boss/");
        }
    }

    private void Start()
    {
        PlayerSpawn();
        EquipLocalWeaponAndArtifacts();
        NextLevel();
    }

    void PlayerSpawn()
    {
        Instantiate(playerPrefab, bossMap);
        camController.AttachPlayerInstance();
    }

    public void NextLevel()
    {
        curLevel++;
        LevelDisplay();
        Inventory.instance.gameMain.hudUI.portionBtn.RestorePotion();

        StartCoroutine(BossSpawnCountdownIE());
    }

    [SerializeField] private TMP_Text countDown_text;
    IEnumerator BossSpawnCountdownIE()
    {
        yield return new WaitForSeconds(2.5f);
        countDown_text.text = "4";
        countDown_text.DOFade(0, 1).From(1);
        yield return new WaitForSeconds(1f);
        countDown_text.text = "3";
        countDown_text.DOFade(0, 1).From(1);
        yield return new WaitForSeconds(1f);
        countDown_text.text = "2";
        countDown_text.DOFade(0, 1).From(1);
        yield return new WaitForSeconds(1f);
        countDown_text.text = "1";
        countDown_text.DOFade(0, 1).From(1);
        yield return new WaitForSeconds(1f);

        // 보스몹 스폰
        int randIdx = UnityEngine.Random.Range(0, bossPrefabs.Length);
        boss = Instantiate(bossPrefabs[randIdx]);
        boss.Init(bossMap.position, 7f);
        boss.heart.SetMonsterStatByLevel((short)curLevel);
        if (bossTrackingCoroutine != null)
        {
            StopCoroutine(bossTrackingCoroutine);
        }

        bossTrackingCoroutine = StartCoroutine(BossTrackingIE(boss));
    }
    
    
    
    private Coroutine bossTrackingCoroutine;

    private IEnumerator BossTrackingIE(Monsters.Monster monster)
    {
        Monsters.Monster boss = monster;
        while (true)
        {
            yield return null;
            if (monster != null)
            {
                if (monster.fsm.CheckCurState(Monsters.FSM.EMonsterState.Dead))
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
        // 보스 사망, next level 포탈 생성
        NextLevelPortal nxtPortal = Instantiate(nextLevelPortal_prefab);
        nxtPortal.transform.position = Player.Instance.transform.position;
        nxtPortal.Activate(true);
    }

    [SerializeField] private CanvasGroup levelCG; // 레벨 UI 투명화용
    [SerializeField] private TMP_Text levelTMP; // 레벨 UI 텍스트 수정용
    private void LevelDisplay()
    {
        levelTMP.text = "Level " + curLevel;
        RectTransform cgRect = levelCG.GetComponent<RectTransform>();
        float yy = cgRect.anchoredPosition.y;
        Sequence levelSequence = DOTween.Sequence();
        levelSequence.Append(levelCG.DOFade(1, 0.5f).From(0))
            .Join(cgRect.DOAnchorPosY(yy - 50, 0.5f))
            .AppendInterval(1.5f)
            .Append(levelCG.DOFade(0, 0.5f).From(1))
            .Join(cgRect.DOAnchorPosY(yy, 0.5f));
    }

    void EquipLocalWeaponAndArtifacts()
    {
        JItemsList itemsList = GameManager.Instance.GetJItems();
        
        ItemData weaponItemData = new ItemData();
        for (int i = 0; i < ItemGenerator.Instance.weaponDatas.Length; i++)
        {
            if (ItemGenerator.Instance.weaponDatas[i].itemName == itemsList.weapon.itemName)
            {
                weaponItemData = ItemGenerator.Instance.weaponDatas[i];
                break;
            }
        }
        
        Dictionary<WeaponKey, float> weaponOptions = new Dictionary<WeaponKey, float>();
        weaponOptions.Add(WeaponKey.ATK, itemsList.weapon.atk);
        weaponOptions.Add(WeaponKey.ATKSPEED, itemsList.weapon.atkspeed);
        weaponOptions.Add(WeaponKey.CRIT_RATE, itemsList.weapon.critrate);
        weaponOptions.Add(WeaponKey.CRIT_DAMAGE, itemsList.weapon.critdamage);
        weaponOptions.Add(WeaponKey.SOCKET, 6);
        
        Weapon newWeapon = new Weapon(weaponItemData, 0, 0, weaponOptions);
        newWeapon.itemName = weaponItemData.itemName;
        Inventory.instance.AddItem(newWeapon);
        
        // 무기 장착
        // WeaponEquip(newWeapon);
        
        
        for (int i = 0; i < itemsList.items.Count; i++)
        {
            ItemData artiItemData = ItemGenerator.Instance.artifactDatas[
                Math.Clamp((itemsList.items[i].itemTier / 5), 0, ItemGenerator.Instance.artifactDatas.Length - 1)];
        
            Artifact newArtifact = new Artifact(artiItemData, (ulong)(i + 1), (short)itemsList.items[i].itemTier,
                itemsList.items[i].itemOptions);
            newArtifact.itemName = itemsList.items[i].itemName;
            Inventory.instance.AddItem(newArtifact);
        }
        
        Player.Instance.heart.PlayerItemEquip();
        GameManager.Instance.RemoveItemsJson();
    }

    void WeaponEquip(Weapon newWeapon)
    {
        
        
    }
}