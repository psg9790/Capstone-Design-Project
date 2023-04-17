using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType type;
    public double atk;
    public double atkspeed;
    public double hp;
    public double def;
    public double speed;
    public string itemName;
    public Sprite itemImage;
}

public enum ItemType
{
    Weapon,
    Armor,
    Artifact,
    Potion
}
