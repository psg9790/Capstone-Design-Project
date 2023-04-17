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
            list.Add((int)EMonsterState.ChasePlayer, new State_ChasePlayer());
            list.Add((int)EMonsterState.Stiff, new State_Stiff());
            list.Add((int)EMonsterState.Die, new State_Die());
            list.Add((int)EMonsterState.Engage, new State_Engage());
            list.Add((int)EMonsterState.KnockBack, new State_KnockBack());
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
    
    public enum EMonsterState // 몬스터의 현재 상태를 나타내기 위한 열거형
    {
        Idle,
        Patrol,
        ChasePlayer,
        Runaway,
        Die,
        Stiff,
        KnockBack,
        Engage
    }
}