using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenBtn : MonoBehaviour
{
    private Button btn;
    public GameObject invenSet;
        // Start is called before the first frame update
    void Start()
    { 
        this.btn = this.GetComponent<Button>();
        // Debug.Log("start");
        this.btn.onClick.AddListener(() =>
        {
            if (invenSet.activeSelf==true)
            {
                invenSet.SetActive(false);
                Debug.Log("button"); 
            }else if(invenSet.activeSelf==false)
                invenSet.SetActive(true);
        });
    }
}
