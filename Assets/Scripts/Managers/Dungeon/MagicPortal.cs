using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPortal : MonoBehaviour
{
    private bool activated = false;
    private Transform target;
    private bool destroyOnTrigger = true;

    public void Activate(Transform tf, bool destroyOnTrigger)
    {
        target = tf;
        this.destroyOnTrigger = destroyOnTrigger;
        // activated = true;
        Invoke("ActivateFlagOn", 1.5f);
    }

    private void ActivateFlagOn()
    {
        activated = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!activated)
            return;
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GrowthLevelManager.Instance.TeleportPlayer(target.position);
            
            if(destroyOnTrigger)
                Destroy(this.gameObject);
        }
    }
}
