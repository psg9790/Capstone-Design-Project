using System;
using System.Collections;
using System.Collections.Generic;
using Monsters;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HPbar_custom : MonoBehaviour
{
    public static HPbar_pool pool;
    [ShowInInspector] public static float GAGE_SPEED = 80f;
    
    private RectTransform rect;
    private Camera cam;
    
    [SerializeField] private Slider red;
    [SerializeField] private Slider yellow;
    
    [ShowInInspector][ReadOnly] private Heart heart;
    private float RED_VALUE;
    private float YELLOW_VALUE;

    private void Awake()
    {
        if (pool == null)
        {
            GameObject cvs = new GameObject("Hpbar_canvas");
            pool = cvs.AddComponent<HPbar_pool>();
        }
        cam = Camera.main;
        rect = GetComponent<RectTransform>();
        pool.Add(this);
    }

    private void Update()
    {
        Positioning();
        Red_Activity();
        Yellow_Activity();
        Visualize();
    }

    private void Positioning()
    {
        rect.transform.position = cam.WorldToScreenPoint(heart.hpbar_pos.position);
    }

    private void Red_Activity()
    {
        if (RED_VALUE < heart.CUR_HP)
        {
            RED_VALUE += Time.deltaTime * GAGE_SPEED * 2f;
        }
        if (RED_VALUE > heart.CUR_HP)
        {
            RED_VALUE = heart.CUR_HP;
        }
    }

    private void Yellow_Activity()
    {
        if (YELLOW_VALUE > RED_VALUE)
        {
            YELLOW_VALUE -= Time.deltaTime * GAGE_SPEED;
        }
        if (YELLOW_VALUE < RED_VALUE)
        {
            YELLOW_VALUE = RED_VALUE;
        }
    }

    private void Visualize()
    {
        red.value = RED_VALUE / heart.MAX_HP;
        yellow.value = YELLOW_VALUE / heart.MAX_HP;
    }

    public void Activate(Heart _heart)
    {
        heart = _heart;
        RED_VALUE = heart.CUR_HP;
        YELLOW_VALUE = heart.CUR_HP;
    }
}