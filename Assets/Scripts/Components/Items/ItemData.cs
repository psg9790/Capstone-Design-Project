using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public Sprite iconSprite;
    public string itemName;
    [Multiline] public string tooltip;
    [EnumToggleButtons] public ItemType itemType;
    [ShowIf("itemType", ItemType.Weapon)] public GameObject weapon_gameObject;
    //
    // [ShowIf("itemType", ItemType.Weapon)] public WeaponType weaponType;
    // [ShowIf("itemType", ItemType.Weapon)] public float weapon_atk;
    // [ShowIf("itemType", ItemType.Weapon)] public float weapon_atkSpeed;
    // [ShowIf("itemType", ItemType.Weapon)] public CC_type weapon_ccType;
    //

    // [ShowIfGroup("itemType", ItemType.Artifact)] public ItemValueType valueType;

}

public enum ItemType
{
    Weapon,
    Artifact
}
//
// public enum WeaponType
// {
//     Sword,
//     LongBow
// }
//
// public enum ItemValueType
// {
//     DEF,
//     HP,
//     MOVEMENTSPEED, 
//     ATK,
//     ATKSPEED
// }