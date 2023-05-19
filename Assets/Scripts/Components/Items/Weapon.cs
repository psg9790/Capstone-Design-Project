using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

public class Weapon : Item
{
    // public List<WeaponKey> weaponKeys;
    // public List<float> weaponValues;

    [ShowInInspector] public Dictionary<WeaponKey, float> options;

    public Weapon(ItemData itemData, ulong id, short tier, Dictionary<WeaponKey, float> options)
    {
        this.itemData = itemData;
        this.id = id;
        this.tier = tier;
        this.options = options;
    }

    // 공격력: 14% (+2%)
    // 공격속도: 1.4 (-0.2)
    // 치명타 확률:
    // 치명타 피해:
    public override List<string> Options_ToString()
    {
        List<string> ret = new List<string>();

        StringBuilder sb = new StringBuilder();
        sb.Append("공격력: ");
        sb.Append(options[WeaponKey.ATK].ToString());
        sb.Append("%");
        sb.Append(" ");
        if (Inventory.instance.tempItem != null)
        {
            sb.Append(CompareAndGetColoredString((Inventory.instance.tempItem as Weapon).options[WeaponKey.ATK],
                options[WeaponKey.ATK]));
        }
        else
        {
            sb.Append(CompareAndGetColoredString(0,
                options[WeaponKey.ATK]));
        }
        ret.Add(sb.ToString());

        sb.Clear();
        sb.Append("공격속도: ");
        sb.Append(options[WeaponKey.ATKSPEED].ToString());
        sb.Append(" ");
        if (Inventory.instance.tempItem != null)
        {
            sb.Append(CompareAndGetColoredString((Inventory.instance.tempItem as Weapon).options[WeaponKey.ATKSPEED],
                options[WeaponKey.ATKSPEED]));
        }
        else
        {
            sb.Append(CompareAndGetColoredString(0,
                options[WeaponKey.ATKSPEED]));
        }
        ret.Add(sb.ToString());

        sb.Clear();
        sb.Append("치명타 확률: ");
        sb.Append(options[WeaponKey.CRIT_RATE].ToString());
        sb.Append(" ");
        if (Inventory.instance.tempItem != null)
        {
            sb.Append(CompareAndGetColoredString((Inventory.instance.tempItem as Weapon).options[WeaponKey.CRIT_RATE],
                options[WeaponKey.CRIT_RATE]));
        }
        else
        {
            sb.Append(CompareAndGetColoredString(0,
                options[WeaponKey.CRIT_RATE]));
        }
        ret.Add(sb.ToString());
        
        sb.Clear();
        sb.Append("치명타 피해: ");
        sb.Append(options[WeaponKey.CRIT_DAMAGE].ToString());
        sb.Append(" ");
        if (Inventory.instance.tempItem != null)
        {
            sb.Append(CompareAndGetColoredString((Inventory.instance.tempItem as Weapon).options[WeaponKey.CRIT_DAMAGE],
                options[WeaponKey.CRIT_DAMAGE]));
        }
        else
        {
            sb.Append(CompareAndGetColoredString(0,
                options[WeaponKey.CRIT_DAMAGE]));
        }
        ret.Add(sb.ToString());
        
        return ret;
    }

    private string CompareAndGetColoredString(float original, float compare)
    {
        string hex = "#BCBCBC";
        float diff = (float)Math.Round(compare - original, 1);
        if (diff > 0) // +
        {
            hex = "#6AA84F";
        }
        else if (diff < 0) // -
        {
            hex = "#CC0000";
        }
        
        StringBuilder sb = new StringBuilder();
        sb.Append("<color=");
        sb.Append(hex);
        sb.Append(">");
        sb.Append("(");
        
        sb.Append(diff.ToString());
        
        sb.Append(")");
        sb.Append("</color>");

        return sb.ToString();
    }
}

public enum WeaponKey
{
    ATK,
    ATKSPEED,
    SOCKET,
    CRIT_RATE,
    CRIT_DAMAGE
}