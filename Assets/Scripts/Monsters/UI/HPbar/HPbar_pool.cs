using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPbar_pool : MonoBehaviour
{
    private RectTransform rect;
    private Canvas canvas;
    private CanvasScaler scaler;

    void Awake()
    {
        rect = this.AddComponent<RectTransform>();
        canvas = this.AddComponent<Canvas>();
        scaler = this.AddComponent<CanvasScaler>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(Screen.currentResolution.width,
            Screen.currentResolution.height);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
    }

    private void OnDestroy()
    {
        HPbar_custom.pool = null;
    }

    public void Add(HPbar_custom bar)
    {
        bar.transform.SetParent(this.transform);
    }
}