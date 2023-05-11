using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthLevelManager : MonoBehaviour
{
    private static GrowthLevelManager instance;
    public static GrowthLevelManager Instance => instance;

    public GameObject playerPrefab; // 플레이어 없으면 생성을 위함
    private Vector3 playerSpawnPoint;
    
    public int worldLevel; // 월드레벨, 처음엔 0

    public Transform dungeon1_spawnPoint;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitGrowthDungeon();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void InitGrowthDungeon()
    {
        // level 0
        worldLevel = 0;

        if (Player.Instance == null)
            Instantiate(playerPrefab);
        
        // 랜덤 던전 선택
        ChooseRandomMap();
        
        // 플레이어 이동
        TeleportPlayer(playerSpawnPoint);
    }

    private void ChooseRandomMap()
    {
        playerSpawnPoint = dungeon1_spawnPoint.position;
    }

    private void TeleportPlayer(Vector3 pos)
    {
        if (Player.Instance == null)
        {
            UnityEngine.Debug.LogWarning("플레이어 없음");
            return;
        }

        Player.Instance.nav.enabled = false;
        Player.Instance.transform.position = pos;
        Player.Instance.nav.enabled = true;
    }
}
