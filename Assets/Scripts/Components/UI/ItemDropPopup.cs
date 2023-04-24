using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemDropPopup : MonoBehaviour
{
    static public ItemDropPopup instance;
        
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);  
        }
        else
        {
            instance = this;
            this.gameObject.SetActive(false);
        }
    }
     
}
