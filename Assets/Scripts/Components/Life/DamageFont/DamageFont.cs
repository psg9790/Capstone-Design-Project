using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageFont : MonoBehaviour
{
    private TMP_Text txt;
    private RectTransform rect;

    private float elapsedTime = 0; // 카운팅용 숫자, duration을 넘어서면 폰트 끄기
    private static float duration = 1f; // 이 폰트가 지속될 시간
    private Vector3 pos; // 폰트가 스폰될 위치
    private Vector2 offset; // 폰트가 스폰 지점으로부터 떨어질 오프셋
    // private CanvasGroup cg; // 

    private Coroutine chasePositionCoroutine; // 타겟 쫓아다니는걸 update에서 빼고 코루틴으로 대체
    public AnimationCurve damagePopEase; // 데미지 액션 인스펙터에서 커스터마이징

    private static float lowestDamage = 1f; // 가장 작은 크기로 출력할 데미지 수치
    private static float highestDamage = 40f; // 가장 큰 크기로 출력할 데미지 수치
    private static float minFontScale = 0.5f; // 최소 데미지 폰트 크기 (scale)
    private static float maxFontScale = 1.6f; // 최대 데미지 폰트 크기 (scale)

    public void Activate(float damage, bool isCritical, Vector3 target, Vector3 randomRange)
    {
        if (ReferenceEquals(txt, null))
        {
            txt = GetComponent<TMP_Text>();
            rect = GetComponent<RectTransform>();
        }
        //
        // if (ReferenceEquals(cg, null))
        // {
        //     cg = GetComponent<CanvasGroup>();
        // }
        
        elapsedTime = 0;
        txt.text = damage.ToString();
        pos = target;
        offset = new Vector2(Random.Range(randomRange.x, randomRange.y), Random.Range(0, randomRange.z));
        rect.transform.position = DamageFontManager.Instance.cam.WorldToScreenPoint(pos) + new Vector3(offset.x, offset.y, 0);

        if (isCritical)
            txt.color = Color.yellow;
        else
            txt.color = Color.white;

        rect.DOScale(Mathf.Lerp(minFontScale, maxFontScale,
            ((damage - lowestDamage) / (highestDamage - lowestDamage))), 0.4f).From(0.1f).SetEase(damagePopEase);
        // cg.DOFade(0, duration).From(1).SetEase(Ease.InQuint); // 서서히 투명화
        chasePositionCoroutine = StartCoroutine(ChasePositionCo());
        DOVirtual.DelayedCall(duration, () => Return());
    }

    IEnumerator ChasePositionCo()
    {
        while (elapsedTime <= duration)
        {
            yield return null;
            rect.transform.position = DamageFontManager.Instance.cam.WorldToScreenPoint(pos) + new Vector3(offset.x, offset.y, 0);
            offset.y += Time.deltaTime * 10f;
        }
    }

    // void Update()
    // {
    //     // elapsedTime += Time.deltaTime;
    //     rect.transform.position = DamageFontManager.Instance.cam.WorldToScreenPoint(pos) + offset;
    //     
    //     // offset += Time.deltaTime * Vector3.up * 50f;
    //     // if (elapsedTime > duration)
    //     // {
    //     //     DamageFontManager.Instance.ReturnDamageFont(this);
    //     // }
    // }

    public void Return()
    {
        if (!ReferenceEquals(chasePositionCoroutine, null))
        {
            StopCoroutine(chasePositionCoroutine);
        }
        DamageFontManager.Instance.ReturnDamageFont(this);
    }
}
