using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilMage_Monster : RangedMonster
{
    public GameObject basicMagic;
    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    public void GenerateBaseMagic()
    {
        BasicMagic magic = Instantiate(basicMagic).GetComponent<BasicMagic>();
        magic.Init(this.transform.position + transform.forward * 0.25f + Vector3.up, 
            transform.forward, 2.5f);
    }
}
