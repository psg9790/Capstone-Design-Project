using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MouseTooltip_TrainingGround : MonoBehaviour
{
    public TMP_Text tooltip;
    private RectTransform rect;
    public Vector2 offset;
    
    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 pos = InputManager.Instance.GetMousePosition();
        pos.y -= Screen.height >> 1;
        pos.x -= Screen.width >> 1;
        pos += offset;
        rect.anchoredPosition = pos;
    }

    public void DisableTooltip()
    {
        tooltip.text = "";
        // Invoke("TooltipFalse", 0.25f);
    }

    private void TooltipFalse()
    {
        tooltip.enabled = false;
    }

    public void EnableTooltip(string _text)
    {
        tooltip.enabled = true;
        tooltip.text = _text;
    }
    
}
