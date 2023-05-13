using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class GrowthLevelManager : MonoBehaviour
{
    private static GrowthLevelManager instance;
    public static GrowthLevelManager Instance => instance;

    public GameObject playerPrefab; // 플레이어 없으면 생성을 위함
    private Vector3 playerSpawnPoint;

    [ReadOnly] public int worldLevel; // 월드레벨, 처음엔 0
    [ReadOnly] public int curWorldMapType;
    [ReadOnly] public int curLevelMobCount = 0;
    [HideInInspector] public UnityEvent decreaseMobCount;

    public Transform dungeon1_spawnPoint;
    public Transform maze_spawnPoint;
    public int mazeIndent = 28;
    public int maxMazeBlockCount = 12;
    
    public GameObject maze_parent;
    public NavMeshSurface maze_parent_nav;
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
        maze_spawnPoint.transform.position += new Vector3(mazeIndent >> 1, 0, mazeIndent >> 1);
        // level 0
        worldLevel = -1;

        if (Player.Instance == null)
            Instantiate(playerPrefab);

        // 랜덤 던전 선택
        MakeRandomMap();

        // 플레이어 이동
        TeleportPlayer(playerSpawnPoint);
    }

    private void UpdateNavMesh()
    {
        maze_parent_nav.UpdateNavMesh(maze_parent_nav.navMeshData);
    }

    public void UpdateNavMeshDelay(float sec)
    {
        Invoke("UpdateNavMesh", sec);
    }

    [Button]
    private void MakeRandomMap()
    {
        worldLevel++;
        curLevelMobCount = 0;

        curWorldMapType = UnityEngine.Random.Range(0, 2);
        curWorldMapType = 1;
        if (curWorldMapType == 0) // 던전1
        {
            playerSpawnPoint = dungeon1_spawnPoint.position;
        }
        else // 미로 랜덤 생성
        {
            playerSpawnPoint = maze_spawnPoint.position;
            if (!ReferenceEquals(randomMazeGenerator, null)) // 기존 미로 삭제
            {
                randomMazeGenerator.Terminate();
                randomMazeGenerator = null;
                // Destroy(maze_parent);
            }

            randomMazeGenerator = new RandomMazeGenerator(maze_parent.transform, maze_parent.transform.position, mazeIndent, maxMazeBlockCount);
            randomMazeGenerator.RandomGenerate();
            
            if (maze_parent_nav.navMeshData == null)
            {
                maze_parent_nav.BuildNavMesh();
            }
            else
            {
                // Invoke("UpdateNavMesh", Time.deltaTime * 1.5f);
                UpdateNavMeshDelay(Time.deltaTime * 1.5f);
            }
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
        
        Camera.main.GetComponent<CameraController>().Attach(Player.Instance);
    }
}