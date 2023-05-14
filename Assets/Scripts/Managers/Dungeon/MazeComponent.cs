using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class MazeComponent : MonoBehaviour
{
    public List<Transform> monsterSpawnPoints = new List<Transform>();
    [SerializeField][Required] private Transform mid_point;

    private int[] dy = new int[4];
    private int[] dx = new int[4];
    
    public int SpawnMonsters()
    {
        
        return 0;
    }

    private RandomMazeGenerator generator;
    public void BuildWalls(RandomMazeGenerator generator)
    {
        this.generator = generator;
        Array.Copy(generator.dy, dy, 4);
        Array.Copy(generator.dx, dx, 4);
        // this.dy = generator.dy;
        // this.dx = generator.dx;
        
        GameObject wallgo = Resources.Load("MazeComponents/Wall/Wall").GameObject();
        for (int i = 0; i < 4; i++)
        {
            // dy[i] = dy[i] >> 1;
            // dx[i] = dx[i] >> 1;
            Vector3 wallPos = mid_point.position + new Vector3(generator.dx[i] >> 1, 0, generator.dy[i] >> 1);
            GameObject newWall = Instantiate(wallgo, wallPos, Quaternion.LookRotation(wallPos - mid_point.position));
            newWall.name = "wall " + this.gameObject.name + i.ToString();
            newWall.layer = LayerMask.NameToLayer("FakeWall");
            newWall.transform.SetParent(this.transform);
        }
    }

    [Button]
    public void CollapseWalls()
    {
        RaycastHit[] hits;
        for (int i = 0; i < 4; i++)
        {
            Vector3 dir = new Vector3(dx[i], 0, dy[i]);
            hits = Physics.RaycastAll(mid_point.position, dir, dir.magnitude, 1 << LayerMask.NameToLayer("FakeWall"));
            UnityEngine.Debug.DrawRay(mid_point.position, dir, Color.red, 3f);
            for (int j = 0; j < hits.Length; j++)
            {
                Destroy(hits[j].transform.gameObject);
            }
        }
        GrowthLevelManager.Instance.UpdateNavMeshDelay(Time.deltaTime * 1.5f);
    }

    public void Terminate()
    {
        Destroy(this.gameObject);
    }
    
}
