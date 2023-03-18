using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Player player;
    public Camera camera;
    private void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.AddPerformed(InputType.RightClick, OnMove);
        }

        player = GetComponent<Player>();
        camera = Camera.main;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Ray ray = camera.ScreenPointToRay(InputManager.Instance.GetMousePosition());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable")))
        {
            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
            player.Move(hit.point);
        }
    }

}
