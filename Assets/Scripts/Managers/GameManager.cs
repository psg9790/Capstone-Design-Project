
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    Player player;
    [SerializeField] GameObject playerPrefab;
    public SceneLoadManager SceneLoadManager;

    public static GameManager Instance
    {
        get { return instance; }
    }
    public Player GetPlayer
    {
        get { return player; }
    }

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
    }
    
    [Sirenix.OdinInspector.Button]
    void CreatePlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogWarning("there is no player prefab");
            return;
        }
        Transform spawn = FindSpawn();
        GameObject go = Instantiate(playerPrefab, spawn.position, spawn.rotation);
        player = go.GetComponent<Player>();
    }
    Transform FindSpawn()
    {
        GameObject spawn = GameObject.Find("SpawnPoint");
        return spawn.transform;
    }
}
