using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Dungeon_Move : MonoBehaviour
{
    public string Dungeon_next;

    private string town = "Town";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load_Next()
    {
        SceneManager.LoadScene(Dungeon_next);
    }

    public void Load_Town()
    {
        SceneManager.LoadScene(town);
    }
}
