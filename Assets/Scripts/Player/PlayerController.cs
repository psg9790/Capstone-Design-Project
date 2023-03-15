using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    static PlayerController instance;

    public static PlayerController Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.AddPerformed(InputType.RightClick, OnMove);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMousePosition());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 2f);
            GameManager.Instance.GetPlayer.MovePlayer(hit.point);
        }
    }

}
