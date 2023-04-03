using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_interact_quit : MonoBehaviour
{
    public GameObject npcText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Text_Exit()
    {
        npcText.SetActive(false);
    }
    
}
