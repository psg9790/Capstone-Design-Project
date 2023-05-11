using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class GrowthLevelManager : MonoBehaviour
{
    private static GrowthLevelManager instance;
    public static GrowthLevelManager Instance => instance;

    public GameObject playerPrefab; // 플레이어 없으면 생성을 위함
    private Vector3 playerSpawnPoint;

    [ReadOnly] public int worldLevel; // 월드레벨, 처음엔 0
    [ReadOnly] public int curLevelMobCount = 0;
    [HideInInspector] public UnityEvent decreaseMobCount;

    public Transform dungeon1_spawnPoint;
    public Transform maze_spawnPoint;
    public int mazeIndent = 28;
    public int maxMazeBlockCount = 12;
    
    private GameObject maze_parent;
    private RandomMazeGenerator randomMazeGenerator;

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitGrowthDungeon();
            decreaseMobCount.AddListener(DecreaseMobCount);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void DecreaseMobCount()
    {
        if (curLevelMobCount > 0)
        {
            curLevelMobCount--;
        }

        if (curLevelMobCount == 0)
        {
            // 보스몹 생성
        }
    }

    private void InitGrowthDungeon()
    {
        // level 0
        worldLevel = -1;

        if (Player.Instance == null)
            Instantiate(playerPrefab);

        // 랜덤 던전 선택
        ChooseRandomMap();

        // 플레이어 이동
        TeleportPlayer(playerSpawnPoint);
    }

    private void ChooseRandomMap()
    {
        worldLevel++;
        curLevelMobCount = 0;

        int randType = UnityEngine.Random.Range(0, 2);
        if (randType == 0) // 던전1
        {
            playerSpawnPoint = dungeon1_spawnPoint.position;
        }
        else // 미로 랜덤 생성
        {
            if (!ReferenceEquals(maze_parent, null)) // 기존 미로 삭제
            {
                Destroy(maze_parent);
            }

            maze_parent = new GameObject("maze_parent");
            randomMazeGenerator = new RandomMazeGenerator(maze_parent.transform, maze_spawnPoint.position, mazeIndent, maxMazeBlockCount);
            randomMazeGenerator.RandomGenerate();
        }

        // // 디버그
        // playerSpawnPoint = dungeon1_spawnPoint.position;
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