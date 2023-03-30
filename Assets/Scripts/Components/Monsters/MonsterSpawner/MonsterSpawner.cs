using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MonsterSpawner : MonoBehaviour
{
    [AssetList(Path="/Prefabs/Monsters/")]
    public List<Monster> monsterList;
    
    // public List<Monster> monsters = new List<Monster>();
    public float patrolRadius = 4f;
    public int spawnAmount = 3;

    [Button]
    public void Spawn()
    {
        if (monsterList.Count == 0)
        {
            Debug.LogWarning("스포너에 스폰할 몬스터를 지정해주세요.");   
            return;
        }
        for (int i = 0; i < spawnAmount; i++)
        {
            Monster monster = Instantiate<Monster>(
                monsterList[Random.Range(0, monsterList.Count - 1)]);
            monster.transform.position = this.transform.position;
            // monster.spawner = this;
            monster.spawnPoint = this.transform.position;
            monster.patrolRadius = this.patrolRadius;
        }
    }

    // public Vector3 GetRandomPosInPatrolRadius()
    // {
    //     Vector3 target = Vector3.zero;
    //     target.y = this.transform.position.y;
    //     target.z += Random.Range(-patrolRadius, patrolRadius);
    //     target.x += Random.Range(-patrolRadius, patrolRadius);
    //     if (target.magnitude > patrolRadius)
    //     {
    //         target = target.normalized;
    //         target *= 3;
    //     }
    //
    //     return this.transform.position + target;
    // }
}