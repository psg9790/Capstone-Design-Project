using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InitLoading : MonoBehaviour
{
    TMP_Text loadingText;
    List<string> list = new List<string>() { "loading.", "loading..", "loading..." };
    void Awake()
    {
        loadingText = GetComponent<TMP_Text>();
    }

    float elapsed = 0;
    void Update()
    {
        elapsed += Time.deltaTime * 2;
        if (elapsed >= 3)
            elapsed = 0;
        if (loadingText.text.CompareTo(list[(int)elapsed]) != 0)
            loadingText.text = list[(int)elapsed];

    }
}
