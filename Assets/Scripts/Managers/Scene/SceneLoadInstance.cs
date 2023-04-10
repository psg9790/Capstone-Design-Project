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
    [SerializeField] private SceneName[] movableScenes;
    private SettingsInstance settings;
    [SerializeField] private GameObject settingsPrefab;

    public void SwitchScene(string tgt)
    {
        foreach(SceneName name in movableScenes)
        {
            if (name.ToString().CompareTo(tgt) == 0)
            {
                SceneManager.LoadScene(name.ToString());
                return;
            }
        }
        // 씬 찾기 실패시
        Debug.LogWarning("Scene " + tgt + " not setted");
    }

    public void SwitchScene(SceneName tgt)
    {
        foreach (SceneName name in movableScenes)
        {
            if (name.CompareTo(tgt) == 0)
            {
                SceneManager.LoadScene(name.ToString());
                return;
            }
        }
        // 씬 찾기 실패시
        Debug.LogWarning("Scene " + tgt + " not setted");
    }

    public void OpenSettingsWindow()
    {
        if (ReferenceEquals(settings, null))
        {
            settings = Instantiate(settingsPrefab).GetComponent<SettingsInstance>();
        }

        settings.gameObject.SetActive(true);
    }

    public void CloseSettingsWindow()
    {
        settings.CancelChangesButton();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

[Serializable]
public enum SceneName
{
    MainTitle,
    InGameMain,
    SoloDungeon,
    RankDungeon
}
