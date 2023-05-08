using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragArtiSlot : MonoBehaviour
{
    static public DragArtiSlot instance;

    public ArtifactUI dragArtiSlot;

    public int beginSlot;
    //public Weapon weaponSlot;

    [SerializeField] private Image artiImage;
    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
    }
    
    public void DragSetImage(Image _itemImage)
    {
        artiImage.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)
    {
        Color color = artiImage.color;
        color.a = _alpha;
        artiImage.color = color;
    }

    
}
