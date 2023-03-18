using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using DG.Tweening;
using Unity.Mathematics;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    public Player player;

    [Sirenix.OdinInspector.Button]
    public void Attach(Player _player)
    {
        player = _player;
        transform.position = player.transform.position + offset;
        transform.rotation = Quaternion.LookRotation(player.transform.position
                                                     - transform.position);
        player.playerMoveEvent.AddListener(OnPlayerMove);
    }

    private Tweener move;
    void OnPlayerMove()
    {
        move.Kill();
        move = transform.DOMove(player.transform.position + offset, 0.3f);
    }
}
