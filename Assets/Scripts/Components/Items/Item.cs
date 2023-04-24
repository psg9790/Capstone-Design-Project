using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    public ItemType type;
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
    Artifact,
    Potion
}
