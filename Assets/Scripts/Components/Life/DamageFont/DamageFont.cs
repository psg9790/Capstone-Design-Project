using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageFont : MonoBehaviour
{
    private TMP_Text txt;
    private RectTransform rect;

    private float elapsedTime = 0; // 카운팅용 숫자, duration을 넘어서면 폰트 끄기
    private static float duration = 1f; // 이 폰트가 지속될 시간
    private Vector3 pos; // 폰트가 스폰될 위치
    private Vector3 offset = Vector3.zero; // 폰트가 스폰 지점으로부터 떨어질 오프셋

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
