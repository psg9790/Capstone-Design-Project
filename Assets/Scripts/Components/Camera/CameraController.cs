using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;

    private void Update()
    {
        if (!ReferenceEquals(GameManager.Instance.GetPlayer, null))
        {
            // 카메라 따라다니기
        }
    }
}
