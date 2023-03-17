using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private SceneName[] movableScenes;

    private void Start()
    {
        GameManager.Instance.sceneLoadManager = this;
    }

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
}

[Serializable]
public enum SceneName
{
    MainTitle,
    InGameMain,
    SoloDungeon,
    RankDungeon
}
