using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BareHand : IWeapon
{
    public Player player;
    public BareHand()
    {
        player = GameManager.Instance.player;
    }
    public void Attack()
    {
        Debug.Log("basic attack");
    }

    public void Reload()
    {
        Debug.Log("basic reload");
    }

    public void SubSkill()
    {
        Debug.Log("basic subskill");
    }
}
