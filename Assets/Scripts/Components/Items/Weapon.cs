using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Weapon : Item
{
    // public List<WeaponKey> weaponKeys;
    // public List<float> weaponValues;

    [ShowInInspector] public Dictionary<WeaponKey, float> options;

    public Weapon(ItemData itemData, ulong id, short tier, Dictionary<WeaponKey, float> options)
    {
        this.itemData = itemData;
        this.id = id;
        this.tier = tier;
        this.options = options;
    }

    public override List<string> Options_ToString()
    {
        return base.Options_ToString();
    }
}

public enum WeaponKey
{
    ATK,
    ATKSPEED,
    SOCKET
}