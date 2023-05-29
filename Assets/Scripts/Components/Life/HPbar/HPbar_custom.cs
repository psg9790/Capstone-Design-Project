using System;
using System.Collections;
using System.Collections.Generic;
using Monsters;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HPbar_custom : MonoBehaviour
{
    private Camera cam;
    [ShowInInspector] public static float GAGE_SPEED = 80f;

    private RectTransform rect;
    
    [SerializeField] private Slider red;
    [SerializeField] private Slider yellow;
    
    [ShowInInspector][ReadOnly] public Heart heart;
    private float RED_VALUE;
    private float YELLOW_VALUE;

    public bool isBoss = false;
    [ShowIf("isBoss")] public TMP_Text bossNameText;

    private void Awake()
    {
        cam = Camera.main;
        rect = GetComponent<RectTransform>();
        if(!isBoss)
            HPbarManager.Instance.Add(this);
    }

    private void Update()
    {
        if(!isBoss)
            Positioning();
        Red_Activity();
        Yellow_Activity();
        Visualize();
    }

    private void Positioning()
    {
        rect.transform.position = cam.WorldToScreenPoint(heart.upper_pos.position);
    }

    private void Red_Activity()
    {
        if (RED_VALUE < heart.CUR_HP)
        {
            RED_VALUE += Time.deltaTime * GAGE_SPEED * 3f;
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