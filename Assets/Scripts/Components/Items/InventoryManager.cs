using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private static InventoryManager inctance;

    public GameObject Sword;
    private void Awake()
    {
        if (inctance == null)
        {
            inctance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        // weaponIn();
    }
    [Button]
    void weaponIn()
    {
        GameObject weapon = Instantiate(Sword);
        
        Player.Instance.weaponManager.SetWeapon(weapon);
    }
    [Button]
    void weaponOut()
    {
        Player.Instance.weaponManager.UnRegisterWeapon();
    }

    public ItemData testData;
    [Button]
    public void DEBUG_weaponIn()
    {
        GameObject weapon = Instantiate(testData.weapon_gameObject);
        Player.Instance.weaponManager.SetWeapon(weapon);
        Player.Instance.weaponManager.item = new Item(testData, 0, 0);
    }
}
