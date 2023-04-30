using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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


    public void GenerateItem(Transform tf, Heart heart)
    {
        // Item gen
        GameObject itemgo = Instantiate(droppedItemPrefab); // 프리팹 생성
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

        bool isWeapon = UnityEngine.Random.Range(0, 100) < weaponItem_dropRatio; // 확률에 따라 무기 드롭, 아니면 아티팩트 생성
        if (isWeapon) // 무기 데이터 생성
        {
            ItemData wData = weaponDatas[UnityEngine.Random.Range(0, weaponDatas.Length)];
            List<WeaponKey> wKeys = new List<WeaponKey>();
            for (int i = 0; i < System.Enum.GetValues(typeof(WeaponKey)).Length; i++)
            {
                wKeys.Add((WeaponKey)i);
            }
            List<float> wVals = new List<float>();
            for (int i = 0; i < System.Enum.GetValues(typeof(WeaponKey)).Length; i++)
            {
                wVals.Add(UnityEngine.Random.Range(0, float.Parse(wDic[heart.LEVEL][((WeaponKey)i).ToString()].ToString())));
            }
            Weapon weapon = new Weapon(wData, id_generate++, heart.LEVEL, wKeys, wVals);
            drop.item = weapon;
        }
        else // 아티팩트 데이터 생성
        {
            List<ArtifactKey> aKeys = new List<ArtifactKey>();
            for (int i = 0; i < System.Enum.GetValues(typeof(ArtifactKey)).Length; i++) // 옵션들을 확률적으로 추가시켜줌
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                {
                    aKeys.Add((ArtifactKey)i);
                }
            }
            if (aKeys.Count == 0) // 하나도 생성되지 않았을 경우 하나는 넣어줌
            {
                aKeys.Add((ArtifactKey)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(ArtifactKey)).Length));
            }
            List<float> aVals = new List<float>();
            for (int i = 0; i < aKeys.Count; i++)
            {
                aVals.Add(UnityEngine.Random.Range(0, float.Parse(aDic[heart.LEVEL][aKeys[i].ToString()].ToString())));
            }
            Artifact arti = new Artifact(artifactDatas[heart.LEVEL], id_generate++, heart.LEVEL, aKeys, aVals);
            drop.item = arti;
        }

        // // Item.cs gen
        // Item item = new Item();
        // item.id = registerId++;
        // item.tier = heart.LEVEL;

        // arti
        // item.itemData = artifactDatas[UnityEngine.Random.Range(0, artifactDatas.Length)];
        // item.itemOption.typeidx = new List<ItemValueType>();
        // item.itemOption.values = new List<float>();
        // int typeidx = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(ItemValueType)).Length);
        // ItemValueType type = (ItemValueType)typeidx;
        // item.itemOption.typeidx.Add(type);
        // float target = UnityEngine.Random.Range(0, float.Parse(dic[heart.LEVEL][type.ToString()].ToString()));
        // item.itemOption.values.Add(target);

        // drop.item = item;
    }
}