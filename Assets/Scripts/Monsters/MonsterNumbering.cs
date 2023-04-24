using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Monsters
{
    public class MonsterNumbering : MonoBehaviour
    {
        private static MonsterNumbering instance;
        public static MonsterNumbering Instance => instance;
        [ShowInInspector][ReadOnly] private uint number = 0;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            instance = this;
        }

        public uint AssignNumber()
        {
            return number++;
        }
        
        private void OnDestroy()
        {
            instance = null;
        }
    }
}