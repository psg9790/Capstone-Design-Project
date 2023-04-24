using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class transwall : MonoBehaviour
{
    public Material[] mat = new Material[2];
    private bool IsTrans = false;
    private float timer = 0f;
    private const float TRHRESHOLD_MAX_TIMER = 0.5f;
    private Coroutine timeCheckCoroutine;
    public void trans()
    {
        if (IsTrans)
        {
            timer = 0f;
            return;
        }
        gameObject.GetComponent<MeshRenderer>().material = mat[1];
        IsTrans = true;
        CheckTimer();
    }
    private IEnumerator CheckTimerCouroutine()
    {
        timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer > TRHRESHOLD_MAX_TIMER)
            {
                reset();
                IsTrans = false;
                break;
            }

            yield return null;
        }
    }
    public void CheckTimer()
    {
        if (timeCheckCoroutine != null)
        {
            StopCoroutine(timeCheckCoroutine);
        }

        timeCheckCoroutine = StartCoroutine(CheckTimerCouroutine());
    }
    public void reset()
    {
        gameObject.GetComponent<MeshRenderer>().material = mat[0];

    }

   
}
