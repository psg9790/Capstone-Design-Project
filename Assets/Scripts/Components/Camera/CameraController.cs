using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Mathematics;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CameraController : MonoBehaviour
{
    [Sirenix.OdinInspector.ReadOnly] public Player player; // 이 카메라가 쫓아다닐 플레이어 정보
    public Vector3 offset = new Vector3(0, 15, -10); // 플레이어에서 떨어질 벡터
    public bool attached = false;
    public bool rotateOnUpdate = false;

    [Button]
    public void Attach(Player _player) // 이 카메라에 플레이어 정보를 넣어주고 추적을 시작
    {
        player = _player;
        transform.position = player.transform.position + offset;
        transform.rotation = Quaternion.LookRotation((player.transform.position + Vector3.up)
                                                     - transform.position);
        attached = true;
    }

    public void AfterAttach()
    {
        transform.position = player.transform.position + offset;
        transform.rotation = Quaternion.LookRotation((player.transform.position + Vector3.up)
                                                     - transform.position);
        attached = true;
    }

    public void Detach()
    {
        attached = false;
    }

    private void LateUpdate()
    {
        if (attached)
        {
            Vector3 diff = (player.transform.position + offset) - transform.position; // (있어야 하는 자리 - 현재 자리) 벡터
            if (diff.magnitude > 0.1f) // 그 크기가 일정이상 커지면
            {
                OnPlayerMove(); // 카메라 움직이기
            }

            if(rotateOnUpdate)
                transform.rotation = Quaternion.LookRotation(player.transform.position
                                                         - transform.position);


            Vector3 direction = (Player.Instance.transform.position - transform.position).normalized;
            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, Mathf.Infinity,
                1 << LayerMask.NameToLayer("WALL"));
            for (int i = 0; i < hits.Length; i++)
            {
                // Debug.Log(hits[i].transform.gameObject.name);
                
                transwall[] obj = hits[i].transform.parent.GetComponentsInChildren<transwall>();
                for (int j = 0; j < obj.Length; j++)
                {
                    obj[j]?.trans();
                }
            }
        }
    }

    private Tweener move; // Tweener 하나로 움직임 관리

    void OnPlayerMove()
    {
        move = transform.DOMove(player.transform.position + offset, 0.3f); // 부드럽게 움직여주는 DoMove
        move.Play(); // 이전걸 덮어씌우고 새로 실행
    }
}