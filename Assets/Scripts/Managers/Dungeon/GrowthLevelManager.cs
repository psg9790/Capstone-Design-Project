using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using Monsters.FSM;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = System.Random;
using Sequence = DG.Tweening.Sequence;

public class GrowthLevelManager : MonoBehaviour
{
    private static GrowthLevelManager instance; // 싱글톤
    public static GrowthLevelManager Instance => instance;

    public GameObject playerPrefab; // 플레이어 없으면 생성용
    private Vector3 playerSpawnPoint; // 코드 내부에서 새로운 스폰포인트 지정용

    // public int maxLevel; // 마지막 레벨
    [ReadOnly] public int worldLevel; // 월드 레벨 (난이도), 처음엔 0
    [ReadOnly] public int curWorldMapType; // 0 ~ 10까지 확률적으로 맵 생성

    [ReadOnly] public int curLevelMonsterCount = 0; // 현재 레벨 완료 및 보스몬스터 생성?을 위한 현재 몹 마릿수
    [ReadOnly] public int curLevelMaxMonsterCount;
    [HideInInspector] public Transform parent_spawnedMonsters;

    public Transform dungeon1_spawnPoint; // 던전 1에서 스폰될 위치
    public Transform dungeon3_spawnPoint;
    public Transform bossRoom_spawnPoint;
    public List<Transform> dungeon1_monsterSpawnPoints = new List<Transform>(); // 던전1 몬스터 소환 포인트
    public List<Transform> dungeon3_monsterSpawnPoints = new List<Transform>(); // 던전3 몬스터 소환 포인트

    public int mazeIndent = 28; // 미로 블럭들 사이의 간격
    public int maxMazeBlockCount = 12; // 최대로 생성할 미로 방의 개수

    [Required] public GameObject dungeon1_parent;
    [Required] public GameObject dungeon3_parent;
    [Required] public GameObject maze_parent; // 동적 생성된 미로 오브젝트 관리용
    [Required] public NavMeshSurface maze_parent_nav; // 동적 네브메시 생성용
    [Required] public MagicPortal magicPortal_prefab;
    [Required] public NextLevelPortal nextLevelPortal_prefab;
    private bool bossPortalGenerated = false;
    private RandomMazeGenerator randomMazeGenerator; // 랜덤 생성기 클래스

    [HideInInspector] public GameObject[] general_monsters; // 프리랩 로드
    [HideInInspector] public GameObject[] boss_monsters; // 프리팹 로드
    [HideInInspector] public GameObject[] ObjectPreFab; //오브젝트 프리팹 로드

    [SerializeField] private CanvasGroup levelCG; // 레벨 UI 투명화용
    [SerializeField] private TMP_Text levelTMP; // 레벨 UI 텍스트 수정용
    public GameObject DirectionalLight;
    private Light light;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            general_monsters = Resources.LoadAll<GameObject>("Monsters/General/");
            boss_monsters = Resources.LoadAll<GameObject>("Monsters/Boss/");
            worldLevel = 0;
            ObjectPreFab = Resources.LoadAll<GameObject>("Props");
            light = DirectionalLight.GetComponent<Light>();

            if (Player.Instance == null)
                Instantiate(playerPrefab);
            NextLevel();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private Coroutine bossTrackingCoroutine;

    private IEnumerator BossTrackingIE(Monsters.Monster monster)
    {
        Monsters.Monster boss = monster;
        while (true)
        {
            yield return null;
            if (monster != null)
            {
                if (monster.fsm.CheckCurState(EMonsterState.Dead))
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        // 보스 사망, next level 포탈 생성
        NextLevelPortal nxtPortal = Instantiate(nextLevelPortal_prefab);
        nxtPortal.transform.position = Player.Instance.transform.position;
        nxtPortal.Activate(true);
    }

    public void DecreaseMonsterCount() // 몬스터 감소 이벤트, 일정 횟수 이상 몹을 해치우면 보스 생성 등 로직 추가 가능
    {
        if (curLevelMonsterCount > 0)
        {
            curLevelMonsterCount--;
        }
        else
        {
            return;
        }

        if (curLevelMonsterCount <= curLevelMaxMonsterCount * 0.1f)
        {
            if (!bossPortalGenerated)
            {
                // 보스맵 포탈 생성
                bossPortalGenerated = true;
                MagicPortal bossPortal = Instantiate(magicPortal_prefab);
                bossPortal.transform.position = Player.Instance.transform.position;
                bossPortal.Activate(bossRoom_spawnPoint, true);

                // 보스 스폰
                // int randIdx = UnityEngine.Random.Range(0, boss_monsters.Length);
                // Monsters.Monster newBoss = Instantiate(boss_monsters[randIdx], bossRoom_spawnPoint)
                //     .GetComponent<Monsters.Monster>();
                // newBoss.Init(bossRoom_spawnPoint.position, 4f);
                // newBoss.heart.SetMonsterStatByLevel((short)worldLevel);
                // if (bossTrackingCoroutine != null)
                // {
                //     StopCoroutine(bossTrackingCoroutine);
                // }
                //
                // bossTrackingCoroutine = StartCoroutine(BossTrackingIE(newBoss));
            }
        }
    }

    private Monsters.Monster boss;
    private void SpawnBoss()
    {
        int randIdx = UnityEngine.Random.Range(0, boss_monsters.Length);
        Monsters.Monster newBoss = Instantiate(boss_monsters[randIdx], bossRoom_spawnPoint)
            .GetComponent<Monsters.Monster>();
        boss = newBoss;
        newBoss.Init(bossRoom_spawnPoint.position, 4f);
        newBoss.heart.SetMonsterStatByLevel((short)worldLevel);
        if (bossTrackingCoroutine != null)
        {
            StopCoroutine(bossTrackingCoroutine);
        }


        bossTrackingCoroutine = StartCoroutine(BossTrackingIE(newBoss));
    }

    private void RemoveBoss()
    {
        if (bossTrackingCoroutine != null)
        {
            StopCoroutine(bossTrackingCoroutine);
        }

        if (boss != null)
        {
            boss.heart.ForceDead();
        }
    }


    [Button]
    public void NextLevel() // 해당 레벨 클리어 후 다음 레벨 진입
    {
        RemoveBoss();
        bossPortalGenerated = false;
        worldLevel++;
        Inventory.instance.gameMain.hudUI.portionBtn.RestorePotion();
        
        // if (worldLevel > maxLevel)
        // {
        //     // 성장형 던전 클리어
        //     // UI 표시 및 획득한 리롤 토큰 저장
        //     Debug.Log("성장형 던전 클리어");
        //     return;
        // }

        ItemGenerator.Instance.RemoveAllItems();
        curLevelMonsterCount = 0;
        LevelDisplay();

        SpawnBoss();
        
        MakeRandomMap();

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
    private void MakeRandomMap() // 랜덤으로 던전1 및 미로던전으로 진입
    {
        if (!ReferenceEquals(randomMazeGenerator, null)) // 기존 미로 삭제
        {
            randomMazeGenerator.Terminate();
            randomMazeGenerator = null;
        }

        if (parent_spawnedMonsters != null)
        {
            Destroy(parent_spawnedMonsters.gameObject);
        }

        parent_spawnedMonsters = new GameObject("parent_spawnedMonsters").transform;

        curWorldMapType = UnityEngine.Random.Range(0, 10);
        if (curWorldMapType < 2) // 던전 1
        {
            DirectionalLight.SetActive(false);
            dungeon1_parent.SetActive(true);
            playerSpawnPoint = dungeon1_spawnPoint.position;

            for (int i = 0; i < dungeon1_monsterSpawnPoints.Count; i++)
            {
                int dun1_monsterCount = UnityEngine.Random.Range(4, 7);
                for (int j = 0; j < dun1_monsterCount; j++)
                {
                    int rndGeneralMonster = UnityEngine.Random.Range(0, general_monsters.Length);
                    Monsters.Monster newMonster = Instantiate(general_monsters[rndGeneralMonster],
                        dungeon1_monsterSpawnPoints[i].transform.position,
                        dungeon1_monsterSpawnPoints[i].transform.rotation).GetComponent<Monsters.Monster>();
                    newMonster.Init(dungeon1_monsterSpawnPoints[i].transform.position, 4f);
                    newMonster.transform.SetParent(parent_spawnedMonsters);
                    newMonster.heart.SetMonsterStatByLevel((short)worldLevel);
                    curLevelMonsterCount++;
                }
            }
        }
        else if (curWorldMapType < 4) // 던전 3
        {
            if (DirectionalLight.activeSelf==false)
            {
                DirectionalLight.SetActive(true);
            }
            
            light.color = Color.white;
            

            dungeon3_parent.SetActive(true);
            playerSpawnPoint = dungeon3_spawnPoint.position;

            for (int i = 0; i < dungeon3_monsterSpawnPoints.Count; i++)
            {
                int dun3_monsterCount = UnityEngine.Random.Range(4, 7);
                for (int j = 0; j < dun3_monsterCount; j++)
                {
                    int rndGeneralMonster = UnityEngine.Random.Range(0, general_monsters.Length);
                    Monsters.Monster newMonster = Instantiate(general_monsters[rndGeneralMonster],
                        dungeon3_monsterSpawnPoints[i].transform.position,
                        dungeon3_monsterSpawnPoints[i].transform.rotation).GetComponent<Monsters.Monster>();
                    newMonster.Init(dungeon3_monsterSpawnPoints[i].transform.position, 4f);
                    newMonster.transform.SetParent(parent_spawnedMonsters);
                    newMonster.heart.SetMonsterStatByLevel((short)worldLevel);
                    curLevelMonsterCount++;
                }
            }
        }
        else // 미로 랜덤 생성
        {
            
            dungeon1_parent.SetActive(false);
            dungeon3_parent.SetActive(false);
            
            if (DirectionalLight.activeSelf==false)
            {
                DirectionalLight.SetActive(true);
            }
            
            light.color = new Color32(System.Convert.ToByte( UnityEngine.Random.Range(0, 255) ), System.Convert.ToByte(UnityEngine.Random.Range(0, 255)), System.Convert.ToByte(UnityEngine.Random.Range(0, 255)), 255);

            randomMazeGenerator = new RandomMazeGenerator(maze_parent.transform, maze_parent.transform.position,
                mazeIndent, maxMazeBlockCount);
            randomMazeGenerator.RandomGenerate();
            playerSpawnPoint = randomMazeGenerator.PlayerSpawnPoint().position;

            if (maze_parent_nav.navMeshData == null)
            {
                maze_parent_nav.BuildNavMesh();
            }
            else
            {
                UpdateNavMeshDelay(Time.deltaTime * 1.5f);
            }
        }

        curLevelMaxMonsterCount = curLevelMonsterCount;
    }


    public void TeleportPlayer(Vector3 pos) // 플레이어 위치 이동 (에이전트 on/off)
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

    [Button]
    public void KangHo_TeleportPlayer() // 플레이어 위치 이동 (에이전트 on/off)
    {
        if (Player.Instance == null)
        {
            UnityEngine.Debug.LogWarning("플레이어 없음");
            return;
        }
        // 보스 스폰
        GameObject kangho = GameObject.Find("kangho");
        kangho.GetComponent<NavMeshSurface>().BuildNavMesh();
                int randIdx = UnityEngine.Random.Range(0, boss_monsters.Length);
                Monsters.Monster newBoss = Instantiate(boss_monsters[randIdx], kangho.transform)
                    .GetComponent<Monsters.Monster>();
                newBoss.Init(new Vector3(-1000, 1000, -1000), 4f);
                newBoss.heart.SetMonsterStatByLevel((short)worldLevel);
                if (bossTrackingCoroutine != null)
                {
                    StopCoroutine(bossTrackingCoroutine);
                }

        Player.Instance.nav.enabled = false;
        Player.Instance.transform.position = new Vector3(-1010, 1000, -1010);
        Player.Instance.nav.enabled = true;

        Camera.main.GetComponent<CameraController>().Attach(Player.Instance);
    }

    private void LevelDisplay()
    {
        levelTMP.text = "Level " + worldLevel;
        RectTransform cgRect = levelCG.GetComponent<RectTransform>();
        float yy = cgRect.anchoredPosition.y;
        Sequence levelSequence = DOTween.Sequence();
        levelSequence.Append(levelCG.DOFade(1, 0.5f).From(0))
            .Join(cgRect.DOAnchorPosY(yy - 50, 0.5f))
            .AppendInterval(1.5f)
            .Append(levelCG.DOFade(0, 0.5f).From(1))
            .Join(cgRect.DOAnchorPosY(yy, 0.5f));
    }
}