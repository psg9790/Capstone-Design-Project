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
    private static GrowthLevelManager instance; // 싱글톤
    public static GrowthLevelManager Instance => instance;

    public GameObject playerPrefab; // 플레이어 없으면 생성용
    private Vector3 playerSpawnPoint; // 코드 내부에서 새로운 스폰포인트 지정용

    [ReadOnly] public int worldLevel; // 월드 레벨(난이도), 처음엔 0
    [ReadOnly] public int curWorldMapType; // 0~10까지 확률적으로 맵 생성
    
    [ReadOnly] public int curLevelMonsterCount = 0; // 현재 레벨 완료 및 보스몬스터 생성?을 위한 현재 몹 마릿수
    // [HideInInspector] public UnityEvent decreaseMobCount; // 몹 갯수 줄이는 이벤트

    public Transform dungeon1_spawnPoint; // 던전 1에서 스폰될 위치
    public int mazeIndent = 28; // 미로 블럭들 사이의 간격
    public int maxMazeBlockCount = 12; // 최대로 생성할 미로 방의 개수
    
    public GameObject maze_parent; // 동적 생성된 미로 오브젝트 관리용
    public NavMeshSurface maze_parent_nav; // 동적 네브메시 생성용
    private RandomMazeGenerator randomMazeGenerator; // 랜덤 생성기 클래스

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitGrowthDungeon(); // 씬 진입시 성장형 던전 초기화
            // decreaseMobCount.AddListener(DecreaseMobCount); // 몹 감소 이벤트 등록
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void DecreaseMonsterCount() // 몬스터 감소 이벤트, 일정 횟수 이상 몹을 해치우면 보스 생성 등 로직 추가 가능
    {
        if (curLevelMonsterCount > 0)
        {
            curLevelMonsterCount--;
        }

        if (curLevelMonsterCount == 0)
        {
            // 보스몹 생성
        }
    }

    private void InitGrowthDungeon() // 씬 진입 시 성장형 던전 초기화용
    {
        // maze_spawnPoint.transform.position += new Vector3(mazeIndent >> 1, 0, mazeIndent >> 1);
        // level 0
        worldLevel = 0;

        if (Player.Instance == null)
            Instantiate(playerPrefab);

        // 랜덤 던전 선택
        MakeRandomMap();

        // 플레이어 이동
        TeleportPlayer(playerSpawnPoint);
    }

    private void UpdateNavMesh() // 네브메시 업데이트
    {
        maze_parent_nav.UpdateNavMesh(maze_parent_nav.navMeshData);
    }

    public void UpdateNavMeshDelay(float sec) // 네브메시 업데이트를 오브젝트들의 생성/삭제가 끝난 이후에 해야하므로 지연실행 시킴
    {
        Invoke("UpdateNavMesh", sec);
    }

    [Button]
    public void NextLevel() // 해당 레벨 클리어 후 다음 레벨 진입
    {
        worldLevel++;
        curLevelMonsterCount = 0;

        MakeRandomMap();
        
        TeleportPlayer(playerSpawnPoint);
    }

    [Button]
    private void MakeRandomMap() // 랜덤으로 던전1 및 미로던전으로 진입
    {
        
        curWorldMapType = UnityEngine.Random.Range(0, 10);
        // curWorldMapType = 5; // force maze
        if (curWorldMapType < 4) // 던전1
        {
            playerSpawnPoint = dungeon1_spawnPoint.position;
        }
        else // 미로 랜덤 생성
        {
            // playerSpawnPoint = maze_spawnPoint.position;
            if (!ReferenceEquals(randomMazeGenerator, null)) // 기존 미로 삭제
            {
                randomMazeGenerator.Terminate();
                randomMazeGenerator = null;
                // Destroy(maze_parent);
            }

            randomMazeGenerator = new RandomMazeGenerator(maze_parent.transform, maze_parent.transform.position, mazeIndent, maxMazeBlockCount);
            randomMazeGenerator.RandomGenerate();
            playerSpawnPoint = randomMazeGenerator.PlayerSpawnPoint().position;
            
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

    private void TeleportPlayer(Vector3 pos) // 플레이어 위치 이동 (에이전트 on/off)
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