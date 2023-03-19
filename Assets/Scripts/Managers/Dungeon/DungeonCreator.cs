using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 스폰, 몬스터 스폰, 오브젝트 스폰 등 총괄
public class DungeonCreator : MonoBehaviour
{
    private Transform spawnPoint;
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
        if(Camera.main != null)
            Camera.main.GetComponent<CameraController>().Attach(player);
    }
}
