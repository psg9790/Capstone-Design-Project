using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeComponent : MonoBehaviour
{
    [SerializeField][Required] private Transform playerSpawnPoint; // 이 미로 블럭 내에서 플레이어가 스폰될 수 있는 위치
    public List<Transform> monsterSpawnPoints = new List<Transform>(); // 몬스터를 스폰할 위치 좌표들
    private Vector3 mid_point; // 이 미로 블럭의 정중앙 위치값, 미로 블럭 클리어 시 4방향의 벽을 부수기 위함

    private int[] dy = new int[4]; // randomGenerator dy 카피
    private int[] dx = new int[4]; // randomGenerator dx 카피

    private int monsterCount;
    
    public int SpawnMonsters() // 몬스터들을 생성하고 마릿수를 반환 (일정 %의 몹 제거시 이벤트 활용을 위함)
    {
        int rst = 0;
        for (int i = 0; i < monsterSpawnPoints.Count; i++)
        {
            int newMonsterCount = Random.Range(1, 3);
            for (int j = 0; j < newMonsterCount; j++)
            {
                int newMonsterIdx = Random.Range(0, GrowthLevelManager.Instance.general_monsters.Length);
                Monsters.Monster newMonster = Instantiate(GrowthLevelManager.Instance.general_monsters[newMonsterIdx], 
                        monsterSpawnPoints[i].transform.position, 
                        monsterSpawnPoints[i].transform.rotation).GetComponent<Monsters.Monster>();
                newMonster.Init(monsterSpawnPoints[i].transform.position, 4f);
                newMonster.mazeComponent = this;
                newMonster.transform.SetParent(this.transform);
                rst++;
            }
        }

        return monsterCount = rst;
    }

    // private RandomMazeGenerator generator; // dy, dx를 가져오기 위한 변수
    public void BuildWalls(RandomMazeGenerator generator) // 미로 블럭 기준으로 처음에는 4방향 진로를 모두 막아둠
    {
        mid_point = transform.position + new Vector3(14, 4, 14);
        
        // this.generator = generator;
        Array.Copy(generator.dy, dy, 4);
        Array.Copy(generator.dx, dx, 4);
        
        GameObject wallgo = Resources.Load("MazeComponents/Wall/Wall").GameObject();
        for (int i = 0; i < 4; i++)
        {
            Vector3 wallPos = mid_point + new Vector3(generator.dx[i] >> 1, 0, generator.dy[i] >> 1);
            GameObject newWall = Instantiate(wallgo, wallPos, Quaternion.LookRotation(wallPos - mid_point));
            newWall.name = "wall " + this.gameObject.name + i.ToString();
            newWall.layer = LayerMask.NameToLayer("WALL");
            newWall.transform.SetParent(this.transform);
        }
    }

    [Button]
    private void CollapseWalls() // 미로 블럭 내 모든 몬스터 제거시 방 오픈?
    {
        RaycastHit[] hits;
        for (int i = 0; i < 4; i++)
        {
            Vector3 dir = new Vector3(dx[i], 0, dy[i]);
            hits = Physics.RaycastAll(mid_point, dir, dir.magnitude, 1 << LayerMask.NameToLayer("Wall"));
            UnityEngine.Debug.DrawRay(mid_point, dir, Color.red, 3f);
            for (int j = 0; j < hits.Length; j++)
            {
                if (hits[i].transform.gameObject.CompareTag("FakeWall"))
                {
                    Destroy(hits[j].transform.gameObject);
                }
            }
        }
        GrowthLevelManager.Instance.UpdateNavMeshDelay(Time.deltaTime * 1.5f);
    }

    public Transform ReturnPlayerSpawnPoint() // 현재 블럭에서 플레이어가 스폰될 수 있는 위치를 반환
    {
        return playerSpawnPoint;
    }

    public void DecreaseMonsterCount()
    {
        if (monsterCount > 0)
        {
            monsterCount--;
            GrowthLevelManager.Instance.DecreaseMonsterCount();   
        }
        CheckCollapseWalls();
    }

    public void CheckCollapseWalls()
    {
        if (monsterCount <= 0)
        {
            CollapseWalls();
        }
    }
    
    public void Terminate() // 미로 블럭 셧다운
    {
        Destroy(this.gameObject);
    }
    
}
