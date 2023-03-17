using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    static SceneLoadManager instance;

    public static SceneLoadManager Instance
    {
        get { return instance; }
    }
    string nextSceneName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [Sirenix.OdinInspector.Button]
    public void CallNextScene(string name)
    {
        Scene prev = SceneManager.GetActiveScene();
        nextSceneName = name;
        StartCoroutine(LoadNextScene());
    }
    IEnumerator LoadNextScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
