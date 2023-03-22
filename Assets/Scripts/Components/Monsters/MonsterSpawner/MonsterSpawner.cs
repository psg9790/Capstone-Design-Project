using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor.Tilemaps;

public class MonsterSpawner : MonoBehaviour
{
    public List<Monster> monsters = new List<Monster>();
    public float patrolRadius = 4f;

    [Button]
    public void Spawn()
    {
        Monster monster = Instantiate<Monster>(monsters[0]);
        monster.transform.position = GetRandomPosInPatrolRadius();
        monster.spawner = this;
    }

    public Vector3 GetRandomPosInPatrolRadius()
    {
        Vector3 target = Vector3.zero;
        target.y = this.transform.position.y;
        target.z += Random.Range(-patrolRadius, patrolRadius);
        target.x += Random.Range(-patrolRadius, patrolRadius);
        if (target.magnitude > patrolRadius)
        {
            target = target.normalized;
            target *= 3;
        }
        
        return this.transform.position + target;
    }
}
