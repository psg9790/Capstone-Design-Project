using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonsterFOV))]
public class MonsterFOVEditor : Editor
{
    // https://nicotina04.tistory.com/197
    private void OnSceneGUI()
    {
        MonsterFOV fow = (MonsterFOV)target;
        if (fow.monster == null)
            return;
        
        Handles.color = fow.monster.playerInSight ? Color.red : Color.white;

        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);

        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
        
        if (fow.monster.playerInSight)
        {
            Handles.DrawLine(fow.transform.position, fow.monster.player.transform.position);
        }
    }
}