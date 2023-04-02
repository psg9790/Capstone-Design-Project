using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuBtn : MonoBehaviour
{
    private Button btn;
    public GameObject menuSet;
    // Start is called before the first frame update
    void Start()
    {
        this.btn = this.GetComponent<Button>();
        Debug.Log("start");
        this.btn.onClick.AddListener(() =>
        {
            Time.timeScale = 0;
            menuSet.SetActive(true);
        });
    }
    
}
