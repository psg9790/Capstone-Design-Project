using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TrainingGrounds
{
    
public class MonsterSpawnBrush : Brush
{
    private Monsters.Monster prefab;
    public MonsterSpawnBrush(TrainingGround _trainingGround) : base(_trainingGround)
    {
    }

    public MonsterSpawnBrush(TrainingGround _trainingGround, Monsters.Monster prefab) : base(_trainingGround)
    {
        this.prefab = prefab;
        trainingGround.brushType = BrushType.MonsterSpawn;
    }

    public override void Enter()
    {
        StringBuilder sb = new StringBuilder("Spawn Monster ");
        sb.Append("<color=\"red\">");
        sb.Append(prefab.name);
        sb.Append("</color>");
        trainingGround.tooltip.EnableTooltip(sb.ToString());
    }

    public override void Execute()
    {
        base.Execute();
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMousePosition());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
        {
            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
            Monsters.Monster monster = UnityEngine.Object.Instantiate<Monsters.Monster>(prefab);
            monster.Init(hit.point, 4f);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
}
