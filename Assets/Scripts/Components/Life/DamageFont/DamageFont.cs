using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageFont : MonoBehaviour
{
    public static float durtaion = 1.5f;
    private TMP_Text txt;
    private RectTransform rect;
    private Vector3 pos;

    private float elapsedTime = 0;
    private static float duration = 2f;
    private Vector3 offset = Vector3.zero;

    public void Activate(Damage dmg, Vector3 target)
    {
        if (ReferenceEquals(txt, null))
        {
            txt = GetComponent<TMP_Text>();
            rect = GetComponent<RectTransform>();
        }
        
        elapsedTime = 0;
        txt.text = dmg.damage.ToString();
        pos = target;
        offset = Vector3.zero;
        if (dmg.isCritical)
        {
            txt.color = Color.red;
        }
        else
        {
            txt.color = Color.white;
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        rect.transform.position = DamageFontManager.Instance.cam.WorldToScreenPoint(pos) + offset;
        offset += Time.deltaTime * Vector3.up * 50f;
        if (elapsedTime > duration)
        {
            DamageFontManager.Instance.ReturnDamageFont(this);
        }
    }
}
