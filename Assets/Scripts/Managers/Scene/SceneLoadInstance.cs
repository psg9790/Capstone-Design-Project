using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

// UI에 드래그해서 적용하기 쉽게 씬마다 배치할 프래팹을 따로 만들었음
// 배열에 이 씬에서 움직일 수 있는 씬을 미리 정해놓으면 그 씬들로 이동가능함
// 세팅창 키는것도 만들까
public class SceneLoadInstance : MonoBehaviour
{
    // [SerializeField] private SceneName[] movableScenes;
    private SettingsInstance settings;
    [SerializeField] private GameObject settingsPrefab;

    public void SwitchScene(string tgt)
    {
        for(int i =0; i < System.Enum.GetValues(typeof(SceneName)).Length; i++)
        {
            if (((SceneName)i).ToString() == tgt)
            {
                SceneManager.LoadScene(tgt.ToString());
                Time.timeScale = 1;
                return;
            }
        }
        // 씬 찾기 실패시
        Debug.LogWarning("Scene " + tgt + " not setted");
    }

    public void SwitchScene(SceneName tgt)
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(SceneName)).Length; i++)
        {
            if ((SceneName)i == tgt)
            {
                SceneManager.LoadScene(tgt.ToString());
                Time.timeScale = 1;
                return;
            }
        }
        // 씬 찾기 실패시
        Debug.LogWarning("Scene " + tgt + " not setted");
    }

    public void OpenSettingsWindow()
    {
        if (ReferenceEquals(SettingsInstance.Instance, null)) // 세팅 프리팹이 존재하지 않으면 생성, 근데 생성 안되어있을 경우는 없을것임 아마
        {
            settings = Instantiate(settingsPrefab).GetComponent<SettingsInstance>();
        }
        SettingsInstance.Instance.gameObject.SetActive(true);
    }

    public void CloseSettingsWindow()
    {
        settings.CancelChangesButton();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}

// 여기에 사용할 씬 이름 똑같이 넣어두면 호출 가능
public enum SceneName
{
    MainTitle,
    Start_Map
}
