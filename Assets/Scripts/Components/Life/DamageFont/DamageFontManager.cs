using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DamageFontManager : MonoBehaviour
{
    private RectTransform rect;
    private Canvas canvas;
    private CanvasScaler scaler;

    private static DamageFontManager instance;
    public static DamageFontManager Instance => instance;

    public Camera cam; // 2D 포지셔닝용
    private Stack<DamageFont> closed = new Stack<DamageFont>(); // pooling

    public GameObject damageFont;

    public void Init()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        
        instance = this;
        cam = Camera.main;

        // 이 hpbar가 위치할 캔버스를 새로 생성
        rect = this.AddComponent<RectTransform>();
        canvas = this.AddComponent<Canvas>();
        scaler = this.AddComponent<CanvasScaler>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(Screen.currentResolution.width,
            Screen.currentResolution.height);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        damageFont = Resources.Load("UI/DamageFont").GameObject();
    }

    private void OnDestroy()
    {
        instance = null;
        cam = null;
    }

    public void GenerateDamageFont(Vector3 target, float damage, bool isCritical, Vector3 randomRange) // 타겟 머리위에 데미지 폰트 띄움
    {
        DamageFont dmgFont;
        if (closed.Count > 0)
        {
            dmgFont = closed.Pop();
            dmgFont.gameObject.SetActive(true);
            dmgFont.transform.SetAsLastSibling();
        }
        else
        {
            // dmgFont = Instantiate(Resources.Load("UI/DamageFont")).GetComponent<DamageFont>();
            dmgFont = Instantiate(damageFont).GetComponent<DamageFont>();
            dmgFont.transform.SetParent(this.transform);
        }


        dmgFont.Activate(damage, isCritical, target, randomRange);
    }

    public void ReturnDamageFont(DamageFont font) // 사용 후에 풀에 반환
    {
        font.gameObject.SetActive(false);
        closed.Push(font);
    }
}