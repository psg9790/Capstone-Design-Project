using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type
    {
        Melle,
        Range,
    }

    public Type type;
    public int damage;
    public float rate;
    public MeshCollider meleeArea;
    public TrailRenderer TrailEffect;

    public void use()
    {
        if (type == Type.Melle)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        TrailEffect.enabled = true;
        
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        
        yield return new WaitForSeconds(0.3f);
        TrailEffect.enabled = false;

    }
}
