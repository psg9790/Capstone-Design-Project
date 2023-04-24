using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropBtn : MonoBehaviour
{
    private Button btn;

    public ItemSlot itemSlot;
    // Start is called before the first frame update
    void Start()
    {
        this.btn = this.GetComponent<Button>();
        this.btn.onClick.AddListener(() =>
        {
            itemSlot = DragSlot.instance.dragSlot;
            DragSlot.instance.dragSlot = null;
        });
    }
}
