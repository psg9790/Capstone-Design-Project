using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MonsterSkillHitBox : MonoBehaviour
{
    public LayerMask targetMask;
    private HashSet<string> hitHash = new HashSet<string>();

    private void OnTriggerEnter(Collider other)
    {
        // other.gameObject.layer는 레이어 인덱스 (ex. 7)
        // targetMask는 인덱스로 시프트까지 계산된 값 (ex. 128)
        // 이상한데..
        if ((1 << other.gameObject.layer) == targetMask)
        {
            if (!hitHash.Contains(other.transform.root.name)) // 최상위부모 이름,,, 히트한 타겟이 해싱되어 있으면 다시 타격 x 
            {
                Debug.Log(transform.root.name+ " attacks " +other.transform.root.name);
                // Debug.Log("hit count: " + hitHash.Count);
                hitHash.Add(other.transform.root.name); // 히트한 타겟 해싱
            }
        }
    }

    public void ClearHash()
    {
        hitHash.Clear();
    }
}