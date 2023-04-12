using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TrainingGrounds
{
    public class MonsterRemoveBrush : Brush
    {
        public MonsterRemoveBrush(TrainingGround _trainingGround) : base(_trainingGround)
        {
        }

        public override void Enter()
        {
            base.Enter();
            trainingGround.brushType = BrushType.MonsterRemove;
            StringBuilder sb = new StringBuilder("<color=\"red\">Remove Monster</color>");
            trainingGround.tooltip.EnableTooltip(sb.ToString());
        }

        public override void Execute()
        {
            base.Execute();
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Monster")))
            {
                Monsters.Monster monster = hit.transform.root.GetComponent<Monsters.Monster>();
                monster.Eliminate();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}