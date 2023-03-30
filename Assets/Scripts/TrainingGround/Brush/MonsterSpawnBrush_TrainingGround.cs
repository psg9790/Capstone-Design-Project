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
    }

    public override void Exit()
    {
        base.Exit();
    }
}
