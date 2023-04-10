using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBrush_TrainingGround : Brush_TrainingGround
{
    public IdleBrush_TrainingGround(TrainingGround _trainingGround) : base(_trainingGround)
    {
    }

    public override void Enter()
    {
        trainingGround.tooltip.DisableTooltip();
        trainingGround.brushType = BrushType.Idle;
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
