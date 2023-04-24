using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPbarManager : MonoBehaviour
{
    // pool 사용할 필요 없나? 어차피 모든 몬스터 위에 떠있긴 함 (주기적으로 생성될 필요가 없음)
    private static HPbarManager instance;
    public static HPbarManager Instance => instance;
    
    private RectTransform rect;
    private Canvas canvas;
    private CanvasScaler scaler;


    public void Init()
    {
        instance = this;
        
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
        instance = null;
    }

    public void Add(HPbar_custom bar)
    {
        bar.transform.SetParent(this.transform);
    }
}