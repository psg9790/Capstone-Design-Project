using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : Item
{
    public List<ArtifactKey> artifactKeys;
    public List<float> artifactValues;
    
    public Artifact(ItemData itemData, ulong id, short tier, List<ArtifactKey> artifactKeys, List<float> artifactValues)
    {
        this.itemData = itemData;
        this.id = id;
        this.tier = tier;
        this.artifactKeys = artifactKeys;
        this.artifactValues = artifactValues;
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
    ATKSPEED
}