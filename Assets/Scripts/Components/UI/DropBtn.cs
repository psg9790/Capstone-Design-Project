using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropBtn : MonoBehaviour
{
    private Button btn;
    public GameObject itemDropPopup;

    public ItemSlotUI itemSlotUI;
    // Start is called before the first frame update
    void Start()
    {
        this.btn = this.GetComponent<Button>();
        this.btn.onClick.AddListener(() =>
        {
            itemSlotUI = DragSlot.instance.dragSlot;
            itemDropPopup.SetActive(false);
            itemSlotUI.isClicked = true;
            DragSlot.instance.dragSlot = null;
        });
    }
}
