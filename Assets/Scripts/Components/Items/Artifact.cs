using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
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
        List<string> ret = new List<string>();

        StringBuilder sb = new StringBuilder();

        if (options.ContainsKey(ArtifactKey.HP))
        {
            sb.Clear();
            sb.Append("체력: ");
            sb.Append(options[ArtifactKey.HP].ToString());
            ret.Add(sb.ToString());
        }
        if (options.ContainsKey(ArtifactKey.DEF))
        {
            sb.Clear();
            sb.Append("방어력: ");
            sb.Append(options[ArtifactKey.DEF].ToString());
            ret.Add(sb.ToString());
        }
        if (options.ContainsKey(ArtifactKey.MOVEMENTSPEED))
        {
            sb.Clear();
            sb.Append("이동속도: ");
            sb.Append(options[ArtifactKey.MOVEMENTSPEED].ToString());
            ret.Add(sb.ToString());
        }
        if (options.ContainsKey(ArtifactKey.ATK))
        {
            sb.Clear();
            sb.Append("공격력: ");
            sb.Append(options[ArtifactKey.ATK].ToString());
            ret.Add(sb.ToString());
        }
        if (options.ContainsKey(ArtifactKey.ATKSPEED))
        {
            sb.Clear();
            sb.Append("공격속도: ");
            sb.Append(options[ArtifactKey.ATKSPEED].ToString());
            ret.Add(sb.ToString());
        }
        if (options.ContainsKey(ArtifactKey.CRIT_RATE))
        {
            sb.Clear();
            sb.Append("치명타 확률: ");
            sb.Append(options[ArtifactKey.CRIT_RATE].ToString());
            ret.Add(sb.ToString());
        }
        
        return ret;
    }
}

[Serializable]
public enum ArtifactKey
{
    HP,
    DEF,
    MOVEMENTSPEED,
    ATK,
    ATKSPEED,
    CRIT_RATE
}