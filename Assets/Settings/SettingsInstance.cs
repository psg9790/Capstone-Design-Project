using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsInstance : MonoBehaviour
{
    public UniversalRenderPipelineAsset asset;

    public TMP_Dropdown resolutions;
    public Toggle fullscreen;

    private void OnEnable()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        // 창 킬때 현재 옵션을 기준으로 파라미터들 설정
    }
    
    public void AcceptChangesButton()
    {
        
    }

    public void CancelChangesButton()
    {
        gameObject.SetActive(false);
    }
}
