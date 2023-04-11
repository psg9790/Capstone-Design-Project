using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Monsters
{
    public class MonsterStateList : MonoBehaviour
    {
        private static MonsterStateList instance;
        public static MonsterStateList Instance => instance;

        [ShowInInspector] public Dictionary<int, MonsterState> list = new Dictionary<int, MonsterState>();

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
            
            list.Add((int)EMonsterState.Idle, new MonsterState_Idle());
            list.Add((int)EMonsterState.Patrol, new MonsterState_Patrol());
            list.Add((int)EMonsterState.Runaway, new MonsterState_Runaway());
            list.Add((int)EMonsterState.BaseAttack, new MonsterState_BaseAttack());
            list.Add((int)EMonsterState.ChasePlayer, new MonsterState_ChasePlayer());
        }
        
        public MonsterState FindState(EMonsterState _state)
        {
            return list[(int)_state];
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}