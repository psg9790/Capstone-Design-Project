using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Mathematics;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    [Sirenix.OdinInspector.ReadOnly] public Player player;

    [Sirenix.OdinInspector.Button]
    public void Attach(Player _player)
    {
        player = _player;
        transform.position = player.transform.position + offset;
        transform.rotation = Quaternion.LookRotation(player.transform.position
                                                     - transform.position);
    }

    private void Update()
    {
        if (player != null)
        {
            Vector3 diff = (player.transform.position + offset) - transform.position;
            if (diff.magnitude > 0.1f)
            {
                OnPlayerMove();
            }
        }
    }

    private Tweener move;
    void OnPlayerMove()
    {
        move = transform.DOMove(player.transform.position + offset, 0.3f);
        move.Play();
    }
}
