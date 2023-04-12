using System;
using System.Collections;
using System.Collections.Generic;
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
        GameObject weapon = Instantiate(Sword);
        Player.Instance.weaponManager.RegisterWeapon(weapon);
        Player.Instance.weaponManager.SetWeapon(weapon);
    }
}
