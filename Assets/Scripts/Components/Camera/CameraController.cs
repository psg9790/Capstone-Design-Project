using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;

    private void Start()
    {
        
    }

    // [Sirenix.OdinInspector.Button]
    // void Attach()
    // {
    //     OnPlayerMove();
    //     transform.rotation = Quaternion.LookRotation(GameManager.Instance.GetPlayer.transform.position - transform.position);
    //     GameManager.Instance.GetPlayer.playerMoveEvent.AddListener(OnPlayerMove);
    // }
    //
    // void OnPlayerMove()
    // {
    //     transform.DOMove(GameManager.Instance.GetPlayer.transform.position + offset, 0.5f);
    // }
    //
}
