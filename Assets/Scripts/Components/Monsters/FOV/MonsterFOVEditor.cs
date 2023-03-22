using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (MonsterFOV))]
public class MonsterFOVEditor : Editor
{
    private void OnSceneGUI()
    {
        MonsterFOV fow = (MonsterFOV)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.red;
        //
        // foreach (Transform visible in fow.visibleTargets)
        // {
        //     Handles.DrawLine(fow.transform.position, visible.transform.position);
        // }
        if (fow.visiblePlayer != null)
        {
            Handles.DrawLine(fow.transform.position, fow.visiblePlayer.transform.position);
        }
    }
}
