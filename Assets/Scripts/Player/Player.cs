using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    IWeapon weapon;

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
    }

    void Update()
    {
        if (InputManager.Instance.LeftClick.WasPerformedThisFrame())
        {
            weapon.Attack();
        }
        if (InputManager.Instance.RightClick.WasPerformedThisFrame())
        {
            weapon.SubSkill();
        }
    }
}
