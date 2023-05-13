using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeComponent : MonoBehaviour
{
    public List<Transform> monsterSpawnPoints = new List<Transform>();
    
    public int SpawnMonsters()
    {
        
        return 0;
    }

    public void Terminate()
    {
        Destroy(this.gameObject);
    }
    
}
