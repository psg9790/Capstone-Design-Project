using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;
using Random = System.Random;

public class ItemGenerator : MonoBehaviour
{
    private static ItemGenerator instance;
    public static ItemGenerator Instance => instance;

    // https://m.blog.naver.com/bbulle/220158917236
    [ShowInInspector] [ReadOnly] private List<Dictionary<string, object>> wDic;
    [ShowInInspector] [ReadOnly] private List<Dictionary<string, object>> aDic;
    [ShowInInspector] [ReadOnly] private ItemData[] artifactDatas;
    [ShowInInspector] [ReadOnly] private ItemData[] weaponDatas;

    private GameObject droppedItemPrefab; // 드랍 아이템 프리팹

    private ulong id_generate = 0;
    private int weaponItem_dropRatio = 10; // 무기 드롭 확률 : 0 ~ 100 %

    StringBuilder sb = new StringBuilder();

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        wDic = CSVReader.Read("ItemData/Randomize/WeaponRadomizeByLevel"); // 무기 랜덤 datasheet 불러옴
        aDic = CSVReader.Read("ItemData/Randomize/ArtifactRandomizeByLevel"); // 아티팩트 랜덤 datasheet 불러옴
        artifactDatas = Resources.LoadAll<ItemData>("ItemData/Artifact/"); // 아티팩트 티어 리스트를 불러옴
        weaponDatas = Resources.LoadAll<ItemData>("ItemData/Weapon/"); // 무기종류 데이터를 불러옴
        droppedItemPrefab = Resources.Load<GameObject>("ItemData/DroppedItem");
    }

    private DroppedItem InstantiateItem(Transform tf)
    {
        GameObject itemgo = Instantiate(droppedItemPrefab); // 프리팹 생성
        itemgo.layer = LayerMask.NameToLayer("Item");
        itemgo.transform.position = tf.position; // 몬스터가 죽은 위치 or 상자 깐 위치에서 생성
        itemgo.transform.rotation = UnityEngine.Random.rotation; // 랜덤 회전값
        DroppedItem drop = itemgo.AddComponent<DroppedItem>(); // 떨어진 아이템 스크립트

        // Item pop action
        Rigidbody rigid = itemgo.GetComponent<Rigidbody>();
        Vector3 popDir = Vector3.up;
        popDir += new Vector3(UnityEngine.Random.Range(0.0f, 1.0f),
            0,
            UnityEngine.Random.Range(0.0f, 1.0f));
        popDir = popDir.normalized;
        popDir *= 15;
        rigid.AddForce(popDir, ForceMode.Impulse); // 아이템이 튕겨나감 (Vector3.up 지향)
        return drop;
    }

    public void GenerateItem(Transform tf, Heart heart)
    {
        DroppedItem drop = InstantiateItem(tf);

        bool isWeapon = UnityEngine.Random.Range(0, 100) < weaponItem_dropRatio; // 확률에 따라 무기 드롭, 아니면 아티팩트 생성
        // 몬스터의 레벨에 따라 수치 다르게 랜덤 생성
        if (isWeapon) // 무기 데이터 생성
        {
            ItemData wData = weaponDatas[UnityEngine.Random.Range(0, weaponDatas.Length)];
            Dictionary<WeaponKey, float> wOptions = new Dictionary<WeaponKey, float>();
            float valRatio = 0;
            for (int i = 0; i < System.Enum.GetValues(typeof(WeaponKey)).Length; i++)
            {
                float valBound = float.Parse(wDic[heart.LEVEL][((WeaponKey)i).ToString()].ToString());
                float randValue = UnityEngine.Random.Range(0, valBound);
                if (randValue / valBound > valRatio)
                {
                    valRatio = randValue / valBound;
                }
                wOptions.Add((WeaponKey)i, (float)Math.Round(randValue, 1));
            }
            wOptions[WeaponKey.SOCKET] = (float)Math.Ceiling(wOptions[WeaponKey.SOCKET]);
            
            Weapon weapon = new Weapon(wData, id_generate++, heart.LEVEL, wOptions);
            weapon.itemName = weapon.itemData.itemName;
            if (valRatio >= 0.96f)
                weapon.itemColor = Color.red;
            else if (valRatio >= 0.89f)
                weapon.itemColor = Color.blue;
            else if (valRatio >= 0.77f)
                weapon.itemColor = Color.green;
            else if (valRatio >= 0.4f)
                weapon.itemColor = Color.yellow;
            else
                weapon.itemColor = Color.white;
            drop.Adjust(weapon);
        }
        else // 아티팩트 데이터 생성
        {
            Dictionary<ArtifactKey, float> aOptions = new Dictionary<ArtifactKey, float>();
            ArtifactKey valKey = ArtifactKey.HP;
            float valRatio = 0;
            for (int i = 0; i < System.Enum.GetValues(typeof(ArtifactKey)).Length; i++) // 옵션들을 확률적으로 추가
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                {
                    float valBound = float.Parse(aDic[heart.LEVEL][((ArtifactKey)i).ToString()].ToString());
                    float randValue = UnityEngine.Random.Range(0, valBound);
                    if (randValue / valBound > valRatio)
                    {
                        valRatio = randValue / valBound;
                        valKey = (ArtifactKey)i;
                    }

                    aOptions.Add((ArtifactKey)i, randValue);
                }
            }

            if (aOptions.Count == 0) // 옵션 최소 1개는 보장
            {
                int randIdx = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(ArtifactKey)).Length);
                float valBound = float.Parse(aDic[heart.LEVEL][((ArtifactKey)randIdx).ToString()].ToString());
                float randVal = UnityEngine.Random.Range(0, valBound);
                valKey = (ArtifactKey)randIdx;
                valRatio = randVal / valBound;
                aOptions.Add((ArtifactKey)randIdx, randVal);
            }

            Artifact arti = new Artifact(artifactDatas[heart.LEVEL], id_generate++, heart.LEVEL, aOptions);

            // 접두어
            sb.Clear();
            switch (valKey)
            {
                case ArtifactKey.HP:
                    sb.Append("풍요의 ");
                    break;
                case ArtifactKey.ATK:
                    sb.Append("파괴의 ");
                    break;
                case ArtifactKey.DEF:
                    sb.Append("강인함의 ");
                    break;
                case ArtifactKey.ATKSPEED:
                    sb.Append("민첩함의 ");
                    break;
                case ArtifactKey.MOVEMENTSPEED:
                    sb.Append("기민함의 ");
                    break;
            }

            sb.Append(arti.itemData.itemName);
            arti.itemName = sb.ToString();

            // 옵션 색
            if (valRatio >= 0.96f)
                arti.itemColor = Color.red;
            else if (valRatio >= 0.89f)
                arti.itemColor = Color.blue;
            else if (valRatio >= 0.77f)
                arti.itemColor = Color.green;
            else if (valRatio >= 0.4f)
                arti.itemColor = Color.yellow;
            else
                arti.itemColor = Color.white;

            drop.Adjust(arti);
        }
    }

    // 플레이어가 인벤토리 바깥으로 아이템 드롭했을 때 바닥에 떨구는 기능
    public void PlayerDropsItem(Transform tf, Item item) // (플레이어 위치, 아이템 객체)
    {
        DroppedItem drop = InstantiateItem(tf);
        drop.Adjust(item);
    }
}