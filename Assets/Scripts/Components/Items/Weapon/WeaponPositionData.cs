using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Position Data", menuName = "Scriptable Object/Weapon Position Data", order = int.MaxValue)]
public class WeaponPositionData : ScriptableObject
{
    public Vector3 localPosition;
    public Vector3 localRotation;
    public Vector3 localScale;
}
