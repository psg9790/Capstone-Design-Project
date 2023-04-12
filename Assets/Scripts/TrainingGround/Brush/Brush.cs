using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrainingGrounds
{
    public class Brush
    {
        protected TrainingGround trainingGround;

        public Brush(TrainingGround _trainingGround)
        {
            trainingGround = _trainingGround;
        }

        public virtual void Enter()
        {
        }

        public virtual void Execute()
        {
        }

        public virtual void Exit()
        {
        }
    }
}