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
    private static SettingsInstance instance;
    public static SettingsInstance Instance => instance;
    
    List<Resolution> resolutions = new List<Resolution>();
    List<Resolution> monitor = new List<Resolution>();
    public UniversalRenderPipelineAsset asset;
    FullScreenMode screenMode;
    //public TMP_Dropdown resolutions;
    Resolution temp;                                 // 해상도 정보를 담기 위한 해상도 변수
    public TMP_Dropdown resolutionDropdown;          // 드롭다운 객체
    public Toggle fullscreen;                       // 전체화면을 위한 토글
    int resolutionNum;                            // 드롭다운 목록에 있는 선택된 해상도를 나타내는 변수
    int[,] resollist = new int[4,2]{{1280,720},{1600,900},{1920,1080},{2560,1440}};       // 해상도 
    private float BgmValue=0.5f;
    private float SFXValue = 0.5f;
    public Slider BgmSlider;                         // bgm 슬라이더 
    public Slider SfxSlider;
        
    public Toggle v_sync;                           // 수직동기화를 위한 토글
    QualitySettings qualitySetting;
    int sync_toggle;                               // 토글의 변한 정보를 담는 변수

    private void Awake()
    {
        if (ReferenceEquals(instance, null))
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        InitUI();
    }

    void InitUI()
    {
        monitor.AddRange(Screen.resolutions);

        temp.refreshRate=60;
        for(int i=0;i<4;i++)
        {
            temp.width=resollist[i,0];
            temp.height=resollist[i,1];
            foreach (Resolution item in monitor)
            {
                if((item.width==temp.width) && (item.height==temp.height) && (item.refreshRate==60)) 
                {
                    resolutions.Add(temp);
                }    
            
            }
        }

        resolutionDropdown.options.Clear();
        int optionNum=0;
        foreach(Resolution item in resolutions)                                         // 해상도 목록을 읽어서 드롭다운 메뉴에 추가
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text=item.width + "x" + item.height ;
            resolutionDropdown.options.Add(option);

            if(item.width == Screen.width && item.height == Screen.height)
                resolutionDropdown.value=optionNum;
            
            optionNum++;
            
        }
        resolutionDropdown.RefreshShownValue();                                         // 드롭다운 메뉴 초기화 
        fullscreen.isOn=Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow)? true:false;
        v_sync.isOn=(QualitySettings.vSyncCount ==1) ? true:false;

        BgmSlider.value = BgmValue;
        SfxSlider.value = SFXValue;
    }

    public void DropboxOptionChange(int x)                                              // 해상도 선택에 따른 변수 값 변경 함수
    {
        resolutionNum=x; 
    }

    private void OnEnable()                                     // 
    {
        LoadSettings();
    }

    public void LoadSettings()                                  //
    { 
        // 창 킬때 현재 옵션을 기준으로 파라미터들 설정
    }

    public void FullToggleBtn(bool isFull)
    {
        screenMode=isFull?FullScreenMode.FullScreenWindow:FullScreenMode.Windowed;
    }
    
    public void SyncToggleBtn(bool isFull_v)
    {
        sync_toggle=isFull_v ? 1:0;
    }

    public void AcceptChangesButton()                           // 확인 버튼을 눌렀을 때 호출되는 함수
    {
        Screen.SetResolution(resolutions[resolutionNum].width,resolutions[resolutionNum].height,screenMode);
        QualitySettings.vSyncCount=sync_toggle;
        BgmValue = BgmSlider.value;
        SFXValue = SfxSlider.value;
        GameManager.Instance.UpdateCanvasInterval(0.5f);
    }

    public void CancelChangesButton()
    {
        
        BgmSlider.value = BgmValue;
        SfxSlider.value = SFXValue;
        gameObject.SetActive(false);
        
    }
}

