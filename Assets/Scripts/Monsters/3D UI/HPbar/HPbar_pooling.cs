using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class HPbar_pooling : MonoBehaviour
{
    private static HPbar_pooling instance;
    public static HPbar_pooling Instance => instance;

    [SerializeField] private HPbar_custom prefab;
    [ShowInInspector] private Stack<HPbar_custom> closed = new Stack<HPbar_custom>();
    public GameObject parent;
    private Canvas _canvas;
    private CanvasScaler _canvasScaler;

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
        _canvas = parent.AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
        _canvasScaler = parent.AddComponent<CanvasScaler>();
        _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        // resolution change event 필요.
        _canvasScaler.referenceResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        _canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        _canvasScaler.matchWidthOrHeight = 0.5f;
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