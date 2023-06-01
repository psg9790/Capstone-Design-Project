using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class AttachTranswall : MonoBehaviour
{
    public Material a;
    public Material b;
    [Button]
    public void Attach()
    {
        MeshRenderer[] renders = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            transwall tw = renders[i].transform.AddComponent<transwall>();
            tw.mat[0] = a;
            tw.mat[1] = b;
        }
    }
}
