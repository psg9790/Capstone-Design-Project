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
    [ShowInInspector][ReadOnly] private List<Dictionary<string, object>> dic;
    [ShowInInspector][ReadOnly] private ItemData[] artifactDatas;
    [ShowInInspector][ReadOnly] private ItemData[] weaponDatas;

    [SerializeField] private GameObject itemObject;
    
    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        
        // 랜덤 데이터 생성
        instance = this;
        dic = CSVReader.Read("ItemData/RandomItem");
        artifactDatas = Resources.LoadAll<ItemData>("ItemData/Artifact/");
        weaponDatas = Resources.LoadAll<ItemData>("ItemData/Weapon/");
    }

    
    public void GenerateItem(Transform tf, Heart heart)
    {
        GameObject gennedItem = Instantiate(itemObject);
        gennedItem.transform.position = tf.position;
        gennedItem.transform.rotation = UnityEngine.Random.rotation;
        DroppedItem drop = gennedItem.AddComponent<DroppedItem>();

        Rigidbody gennedItemRigid = gennedItem.GetComponent<Rigidbody>();
        Vector3 popDir = Vector3.up;
        popDir += new Vector3(UnityEngine.Random.Range(0.0f, 1.0f),
            0,
            UnityEngine.Random.Range(0.0f, 1.0f));
        popDir = popDir.normalized;
        popDir *= 10;
        gennedItemRigid.AddForce(popDir, ForceMode.Impulse);
        // Debug.Log(popDir);

        Item item = new Item();
        item.itemData = artifactDatas[UnityEngine.Random.Range(0, artifactDatas.Length)];
        
        // arti
        item.itemOption.typeidx = new List<ItemValueType>();
        item.itemOption.values = new List<float>();
        
        int typeidx = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(ItemValueType)).Length);
        ItemValueType type = (ItemValueType)typeidx;
        item.itemOption.typeidx.Add(type);
        float target = UnityEngine.Random.Range(0, float.Parse(dic[heart.LEVEL][type.ToString()].ToString()));
        item.itemOption.values.Add(target);

        drop.item = item;
    }
}
