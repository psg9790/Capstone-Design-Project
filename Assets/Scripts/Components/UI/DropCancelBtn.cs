using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropCancelBtn : MonoBehaviour
{
    private Button btn;
    public GameObject itemDropPopup;
    // Start is called before the first frame update
    void Start()
    {
        this.btn = this.GetComponent<Button>();
        this.btn.onClick.AddListener(() =>
        {
            itemDropPopup.SetActive(false);
            DragSlot.instance.dragSlot = null;
        });
    }
}
