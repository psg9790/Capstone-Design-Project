using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMazeGenerator
{
    private int[] dy = new[] { -1, 1, 0, 0 };
    private int[] dx = new[] { 0, 0, -1, 1 };
    
    private Vector3 zeroPos;
    private Transform parentTransform;
    private int roomCount;
    
    private HashSet<Vector3> closed = new HashSet<Vector3>();
    private HashSet<Vector3> openedHash = new HashSet<Vector3>();
    private List<Vector3> opened = new List<Vector3>();
    private GameObject[] mazePrefabs;

    public RandomMazeGenerator(Transform parent, Vector3 pos, int blockSize, int roomCount)
    {
        parentTransform = parent;
        zeroPos = pos;
        this.roomCount = roomCount;
        mazePrefabs = Resources.LoadAll<GameObject>("MazeComponents/");

        for (int i = 0; i < 4; i++) // dy, dx 수정
        {
            dy[i] *= blockSize;
            dx[i] *= blockSize;
        }
        opened.Add(zeroPos);
        openedHash.Add(zeroPos);
    }

    public void RandomGenerate()
    {
        while (closed.Count < roomCount)
        {
            int randOpen = UnityEngine.Random.Range(0, opened.Count);
            Vector3 popPos = opened[randOpen];
            opened.RemoveAt(randOpen);
            
            if(!closed.Contains(popPos))
            {
                int randBlock = UnityEngine.Random.Range(0, mazePrefabs.Length); // 랜덤 블록 인덱스
                GameObject newBlock = Object.Instantiate(mazePrefabs[randBlock]); // 랜덤 블록 생성
                newBlock.transform.SetParent(parentTransform);
                newBlock.transform.position = popPos; // 위치 설정
                closed.Add(popPos);
                for (int i = 0; i < 4; i++)
                {
                    Vector3 dd = popPos + new Vector3(dx[i], 0, dy[i]);
                    if (!openedHash.Contains(dd))
                    {
                        opened.Add(dd);
                        openedHash.Add(dd);
                    }
                }
                if (newBlock.TryGetComponent<MazeComponent>(out MazeComponent newMaze))
                {
                    GrowthLevelManager.Instance.curLevelMobCount += newMaze.SpawnMonsters();
                }
            }
            if (opened.Count == 0)
                break;
        }
        opened.Clear();
        openedHash.Clear();
        closed.Clear();
    }
}
