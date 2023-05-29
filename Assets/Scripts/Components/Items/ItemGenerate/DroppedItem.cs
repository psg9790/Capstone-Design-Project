using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [ShowInInspector] public Item item;
    public ParticleSystem dropEffect;

    public void Adjust(Item item)
    {
        this.item = item;
        gameObject.name = item.itemName;
        VFXColorChanger(item.itemColor);
    }

    private void VFXColorChanger(Color color)
    {
        Vector4 target = color;
        ParticleSystem[] particles = dropEffect.GetComponentsInChildren<ParticleSystem>();
        var imain = particles[0].main;
        imain.startColor = (Color)target;
        target = new Vector4(Math.Clamp(target.x + 0.4f, 0, 1),
            Math.Clamp(target.y + 0.4f, 0, 1),
            Math.Clamp(target.z + 0.4f, 0, 1),
            Math.Clamp(target.w + 0.4f, 0, 1));

        
        
        for (int i = 1; i < particles.Length; i++)
        {
            var main = particles[i].main;
            main.startColor = (Color)target;
        }
    }
}
