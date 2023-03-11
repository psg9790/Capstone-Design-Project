using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BareHand : IWeapon
{
    public Player player;
    public BareHand()
    {
        player = GameManager.instance.player;
    }
    public void Attack()
    {
        Debug.Log("basic attack");
        Debug.Log(player.gameObject.name);
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
