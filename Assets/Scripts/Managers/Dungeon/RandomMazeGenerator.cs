using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMazeGenerator
{
    public int[] dy = new[] { -1, 1, 0, 0 };
    public int[] dx = new[] { 0, 0, -1, 1 };

    private Vector3 zeroPos; // 맨 처음으로 생성할 미로 블럭의 위치
    private Transform parentTransform; // 생성한 미로 오브젝트들을 계층화 시키기 위한 transform 부모
    private int roomCount; // 생성할 방의 개수

    private HashSet<Vector3> closed = new HashSet<Vector3>(); // 이미 생성을 완료해서 사용할 수 없는 위치들

    private HashSet<Vector3>
        openedHash = new HashSet<Vector3>(); // 이미 추가한 위치 후보를 다시 추가하지 않게 하기 위함 (opened에서 랜덤으로 추출하는 데에 성능이슈 우려)

    private List<Vector3> opened = new List<Vector3>(); // 랜덤 생성할 수 있는 위치 후보들
    private MazeComponent[] mazePrefabs; // 랜덤 미로 생성에 사용할 리소스들

    public List<MazeComponent> mazeComponents = new List<MazeComponent>(); // 생성된 미로 블럭들의 컴포넌트들

    public RandomMazeGenerator(Transform parent, Vector3 pos, int blockSize, int roomCount) // 생성자
    {
        parentTransform = parent; // 미로 블록들이 소속될 오브젝트
        zeroPos = pos; // 처음 위치
        this.roomCount = roomCount; // 생성할 블록 개수
        mazePrefabs = Resources.LoadAll<MazeComponent>("MazeComponents/"); // 미로 블록 로딩

        for (int i = 0; i < 4; i++) // dy, dx 수정
        {
            dy[i] *= blockSize;
            dx[i] *= blockSize;
        }

        opened.Add(zeroPos); // 초기 위치 설정
        openedHash.Add(zeroPos);
    }

    public void RandomGenerate() // 미로 블럭들 랜덤 생성
    {
        while (closed.Count < roomCount)
        {
            int randOpen = UnityEngine.Random.Range(0, opened.Count); // 후보 리스트 중 하나 랜덤 선택
            Vector3 popPos = opened[randOpen];
            opened.RemoveAt(randOpen);

            if (!closed.Contains(popPos)) // 이미 설치된 적이 없어야함
            {
                int randBlock = UnityEngine.Random.Range(0, mazePrefabs.Length); // 랜덤 블록 리소스 인덱스
                MazeComponent newMaze = Object.Instantiate(mazePrefabs[randBlock]); // 블록 생성
                newMaze.transform.position = popPos; // 위치 설정
                closed.Add(popPos); // 설치 처리
                for (int i = 0; i < 4; i++) // 새로운 위치정보들을 추가
                {
                    Vector3 dd = popPos + new Vector3(dx[i], 0, dy[i]);
                    if (!openedHash.Contains(dd))
                    {
                        opened.Add(dd);
                        openedHash.Add(dd);
                    }
                }

                mazeComponents.Add(newMaze); // 생성된 블록들 리스트에 추가
                GrowthLevelManager.Instance.curLevelMonsterCount += newMaze.SpawnMonsters(); // 몬스터 생성 - 전체 던전의 몬스터 마릿수 증가
                newMaze.SpawnTreasure();
                newMaze.SpawnObjects();

                newMaze.transform.SetParent(parentTransform); // 미로 블록을 설정한 오브젝트의 하위로 배치
            }

            if (opened.Count == 0)
                break;
        }

        BuildWalls(); // 미로의 벽 생성 - 몬스터를 처치하지 않으면 다음 블록으로 이동할 수 없게
        // 해시 초기화
        opened.Clear();
        openedHash.Clear();
        closed.Clear();
    }

    private void BuildWalls() // 미로 블록의 벽 생성
    {
        for (int i = 0; i < mazeComponents.Count; i++)
        {
            mazeComponents[i].BuildWalls(this);
        }
    }

    public Transform PlayerSpawnPoint() // 가장 처음 생성한 블럭의 플레이어 생성 지점을 반환
    {
        return mazeComponents[0].ReturnPlayerSpawnPoint();
    }

    public void Terminate() // 생성한 랜덤 미로를 완전히 종료(삭제)함
    {
        for (int i = 0; i < mazeComponents.Count; i++)
        {
            mazeComponents[i].Terminate();
        }
    }
}