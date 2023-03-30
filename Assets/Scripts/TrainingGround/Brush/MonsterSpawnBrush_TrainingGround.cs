using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MonsterSpawnBrush_TrainingGround : Brush_TrainingGround
{
    private Monster prefab;
    public MonsterSpawnBrush_TrainingGround(TrainingGround _trainingGround) : base(_trainingGround)
    {
    }

    public MonsterSpawnBrush_TrainingGround(TrainingGround _trainingGround, Monster prefab) : base(_trainingGround)
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
            Monster monster = UnityEngine.Object.Instantiate<Monster>(prefab);
            monster.Spawn(hit.point, 4f);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
