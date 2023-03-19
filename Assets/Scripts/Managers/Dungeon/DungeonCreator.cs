using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    Transform spawnPoint;
    public GameObject playerPrefab;
    [Sirenix.OdinInspector.ReadOnly] public Player player;

    private void Start()
    {
        if (playerPrefab == null)
        {
            UnityEngine.Debug.Log("no playerPrefab");
            return;
        }
        spawnPoint = GameObject.Find("SpawnPoint").transform;
        player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<Player>();
        Camera.main.GetComponent<CameraController>().Attach(player);
    }
}
