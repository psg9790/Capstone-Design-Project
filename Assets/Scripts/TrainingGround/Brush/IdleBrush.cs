using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrainingGrounds
{
    public class IdleBrush : Brush
    {
        public IdleBrush(TrainingGround _trainingGround) : base(_trainingGround)
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
}