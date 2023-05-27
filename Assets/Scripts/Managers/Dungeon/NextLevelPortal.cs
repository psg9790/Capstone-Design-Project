using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelPortal : MonoBehaviour
{
    private bool activated = false;
    private bool destroyOnTrigger = true;

    public void Activate(bool destroyOnTrigger)
    {
        this.destroyOnTrigger = destroyOnTrigger;
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
            if (GrowthLevelManager.Instance != null)
            {
                GrowthLevelManager.Instance.NextLevel();
            }
            else if (RecordLevelManager.Instance != null)
            {
                RecordLevelManager.Instance.NextLevel();
            }
            
            if (destroyOnTrigger)
                Destroy(this.gameObject);
        }
    }
}