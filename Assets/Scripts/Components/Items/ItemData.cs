using System.Collections;
using System.Collections.Generic;
using Monsters;
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

    public int ID => _id;

    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [Multiline] 
    [SerializeField] private string _toolip;
    [SerializeField] private Sprite _iconSprite;
    [SerializeField] private GameObject _dropItemPrefab;
    
    
}

public enum ItemType
{
    Weapon,
    Armor,
    Artifact,
    Potion
}
