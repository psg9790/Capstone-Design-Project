using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolTimeHelper : MonoBehaviour
{
    private static MonoBehaviour monoInstance;
    
    [RuntimeInitializeOnLoadMethod]
    private static void Initializer()
    {
        monoInstance = new GameObject($"[{nameof(CoolTimeHelper)}]").AddComponent<CoolTimeHelper>();
        DontDestroyOnLoad(monoInstance.gameObject);
    }
    
    public new static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoInstance.StartCoroutine(coroutine);
    }

    public new static void StopCoroutine(Coroutine coroutine)
    {
        monoInstance.StopCoroutine(coroutine);
    }
}
