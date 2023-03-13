
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public Player player;
    [SerializeField] GameObject playerPrefab;

    public static GameManager Instance
    {
        get { return instance; }
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
    void CreatePlayer()
    {
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
