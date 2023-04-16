using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class HPbar_pooling : MonoBehaviour
{
    private static HPbar_pooling instance;
    public static HPbar_pooling Instance => instance;

    [SerializeField] private HPbar_custom prefab;
    [ShowInInspector] private Stack<HPbar_custom> closed = new Stack<HPbar_custom>();
    public GameObject parent;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        prefab = (Resources.Load("Hpbar/Hpbar") as GameObject).GetComponent<HPbar_custom>();
        parent = new GameObject("hpbars");
        parent.transform.SetParent(this.transform);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public HPbar_custom Get_HPbar(Heart _heart)
    {
        if (closed.Count > 0)
        {
            HPbar_custom ret = closed.Pop();
            ret.Activate(_heart);
            return ret;
        }

        HPbar_custom crt = Instantiate(prefab);
        crt.Activate(_heart);
        crt.transform.SetParent(parent.transform);
        return crt;
    }

    public void Return_HPbar(HPbar_custom bar)
    {
        closed.Push(bar);
    }
}