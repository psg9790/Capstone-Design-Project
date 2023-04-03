using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// 플레이어 스폰, 몬스터 스폰, 오브젝트 스폰 등 총괄
public class DungeonCreator : MonoBehaviour
{
    private GameObject spawnPoint; // 플레이어가 스폰될 포인트. 씬에 "SpawnPoint" 오브젝트가 있어야함
    public GameObject playerPrefab; // 소환할 플레이어. 소환할 플레이어 프리팹을 인스펙터에서 넣어줄 것
    [HideInInspector] public Player player; // 소환한 플레이어 정보 
    public MonsterSpawner[] spawners;

    private void Awake()
    {
        spawnPoint = GameObject.Find("SpawnPoint");
        if (spawnPoint == null)
        {
            Debug.LogError("플레이어 스폰 지점이 없습니다... \"SpawnPoint\"오브젝트를 플레이어 생성을 원하는 곳에 배치해주세요.");
        }
        if (playerPrefab == null)
        {
            Debug.LogError("소환할 플레이어 프리팹을 지정해주세요...");
        }
        else
        {
            player = Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation)
                .GetComponent<Player>();
        }
    }

    private void Start()
    {


        if (Camera.main != null)
        {
            if (Camera.main.TryGetComponent<CameraController>(out CameraController cameraController))
            {
                cameraController.Attach(player); // 플레이어 기준으로 쿼터뷰 카메라 적용
            }
            else
            {
                Debug.LogError("카메라에 CameraController 컴포넌트를 추가해주세요");
            }
        }

        spawners = FindObjectsOfType<MonsterSpawner>();
    }
}