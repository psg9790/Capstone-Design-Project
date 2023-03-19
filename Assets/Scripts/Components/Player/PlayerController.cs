using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player player;
    private Camera cam;
    private bool rightClickHold = false;
    private void Start()
    {
        if (InputManager.Instance != null)
        {
            // InputManager.Instance.AddPerformed(InputType.RightClick, OnMove);
            InputManager.Instance.AddPerformed(InputType.RightClick, RighClickPerformed);
            InputManager.Instance.AddCanceled(InputType.RightClick, RighClickCanceled);
        }

        player = GetComponent<Player>();
        cam = Camera.main;
    }

    private void Update()
    {
        if (rightClickHold)
        {
            Ray ray = cam.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
                player.Move(hit.point);
            }
        }   
    }

    void RighClickPerformed(InputAction.CallbackContext context)
    {
        rightClickHold = true;
    }
    
    void RighClickCanceled(InputAction.CallbackContext context)
    {
        rightClickHold = false;
    }
    // public void OnMove(InputAction.CallbackContext context)
    // {
    //     Ray ray = camera.ScreenPointToRay(InputManager.Instance.GetMousePosition());
    //     RaycastHit hit;
    //     if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
    //     {
    //         Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
    //         player.Move(hit.point);
    //     }
    // }

}
