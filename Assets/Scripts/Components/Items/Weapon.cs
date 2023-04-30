using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public List<WeaponKey> weaponKeys;
    public List<float> weaponValues;

    public Weapon(ItemData itemData, ulong id, short tier, List<WeaponKey> weaponKeys, List<float> weaponValues)
    {
        this.itemData = itemData;
        this.id = id;
        this.tier = tier;
        this.weaponKeys = weaponKeys;
        this.weaponValues = weaponValues;
    }

    public override List<string> Options_ToString()
    {
        return base.Options_ToString();
    }
}

public enum WeaponKey
{
    ATK,
    ATKSPEED
}