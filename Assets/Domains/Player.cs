using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    IWeapon weapon;

    private void Start()
    {
        if (GameManager.instance.player == null)
        {
            GameManager.instance.player = this;
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
        if (InputManager.instance.LeftClick.WasPerformedThisFrame())
        {
            weapon.Attack();
        }
        if (InputManager.instance.RightClick.WasPerformedThisFrame())
        {
            weapon.SubSkill();
        }
    }
}
