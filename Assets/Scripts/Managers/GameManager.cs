
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // singleton
    static GameManager instance;
    
    // important
    public SceneLoadManager sceneLoadManager;
    public ISave save = new JsonSave();
    //
    // // props
    // [SerializeField] GameObject playerPrefab;
    // Player player;
    
    public static GameManager Instance
    {
        get { return instance; }
    }
    // public Player GetPlayer
    // {
    //     get { return player; }
    // }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogWarning("GameManager cannot be two : Deleted.");
        }

        Application.targetFrameRate = 60;
    }
}
