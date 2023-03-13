using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Vector3 targetPos;

    private void Start()
    {
        InputManager.Instance.AddPerformed(InputType.RightClick, RightClick);
        InputManager.Instance.AddStarted(InputType.LeftClick, LeftClick);
    }
    private void OnDestroy()
    {
        InputManager.Instance.RemovePerformed(InputType.RightClick, RightClick);
        InputManager.Instance.RemoveStarted(InputType.LeftClick, LeftClick);
    }

    void Update()
    {
        // 이동
    }

    void RightClick(InputAction.CallbackContext context)
    {
        Debug.Log("Right Click");
        // 좌표 따고
    }
    void LeftClick(InputAction.CallbackContext context)
    {
        Debug.Log("Left Click");
        Debug.Log(context.phase);
    }
}
