using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenCancelBtn : MonoBehaviour
{
    // Start is called before the first frame update
    private Button btn;
    public GameObject invenSet;
    // Start is called before the first frame update
    void Start()
    {
        this.btn = this.GetComponent<Button>();
        this.btn.onClick.AddListener(() =>
        {
            invenSet.SetActive(false);
        });
    }
}
