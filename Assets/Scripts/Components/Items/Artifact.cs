using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Artifact : Item
{
    // public List<ArtifactKey> artifactKeys;
    // public List<float> artifactValues;

    [ShowInInspector] public Dictionary<ArtifactKey, float> options;

    public Artifact(ItemData itemData, ulong id, short tier, Dictionary<ArtifactKey, float> options)
    {
        this.itemData = itemData;
        this.id = id;
        this.tier = tier;
        this.options = options;
    }

    public override List<string> Options_ToString()
    {
        return base.Options_ToString();
    }
}

public enum ArtifactKey
{
    HP,
    DEF,
    MOVEMENTSPEED,
    ATK,
    ATKSPEED,
    CRIT_RATE
}