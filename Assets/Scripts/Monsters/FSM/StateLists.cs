using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Monsters.FSM
{
    public class StateLists : MonoBehaviour
    {
        private static StateLists instance;
        public static StateLists Instance => instance;

        [ShowInInspector] public Dictionary<int, State> list = new Dictionary<int, State>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
            
            list.Add((int)EMonsterState.Idle, new State_Idle());
            list.Add((int)EMonsterState.Patrol, new State_Patrol());
            list.Add((int)EMonsterState.Runaway, new State_Runaway());
            list.Add((int)EMonsterState.BaseAttack, new State_BaseAttack());
            list.Add((int)EMonsterState.ChasePlayer, new State_ChasePlayer());
        }
        
        public State FindState(EMonsterState _state)
        {
            return list[(int)_state];
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}