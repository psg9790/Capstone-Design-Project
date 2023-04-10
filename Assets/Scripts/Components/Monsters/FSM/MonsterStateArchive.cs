using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Monsters
{
    public class MonsterStateArchive : MonoBehaviour
    {
        private static MonsterStateArchive instance;
        public MonsterStateArchive Instance => instance;
        
        
        void Awake()
        {
            instance = this;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        public MonsterState FindState(EMonsterState _state)
        {
            return null;
        }
    }
}