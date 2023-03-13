using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    IWeapon weapon;
    Vector3 targetPos;

    private void Start()
    {
        if (GameManager.Instance.player == null)
        {
            GameManager.Instance.player = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        weapon = new BareHand();

        InputManager.Instance.AddPerform("RightClick", RightClick);
        // InputManager.Instance.RemovePerform("RightClick", RightClick);

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
}
