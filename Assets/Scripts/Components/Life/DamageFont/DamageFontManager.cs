using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFontManager : MonoBehaviour
{
    private static DamageFontManager instance;
    public static DamageFontManager Instance => instance;

    private Camera cam; // 카메라 해싱용
    public DamageFont prefab; // 데미지폰트 프리팹
    private List<DamageFont> closed = new List<DamageFont>(); // pooling

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        cam = Camera.main;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void GenerateDamageFont(GameObject target, float damage, bool isCritical) // 머리위에 데미지 폰트 띄움
    {
        if (closed.Count > 0)
        {
            return;
        }

        DamageFont dmgfont = Instantiate(prefab);
        dmgfont.GetComponent<RectTransform>().transform.position =
            cam.WorldToScreenPoint(target.transform.position);
        dmgfont.Init(damage, isCritical);
    }

    public void ReturnDamageFont(DamageFont font) // 사용 후에 풀에 반환
    {
        font.gameObject.SetActive(false);
        closed.Add(font);
    }
}