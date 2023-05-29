using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class BeforeRecordDungeonUI : MonoBehaviour
{
    public List<Transform> itemDisplays = new List<Transform>();
    public TMP_Text diceCountText;
    public TMP_Text statCombinationText;
    public TMP_Dropdown weapon_dropdown;

    private void Start()
    {
        WeaponUI_Init();
        RefreshItemUI();
        GameManager.Instance.itemChangedEvent.AddListener(RefreshItemUI);
    }

    private void OnEnable()
    {
        WeaponUI_Init();
        RefreshItemUI();
    }

    private void OnDestroy()
    {
        GameManager.Instance.itemChangedEvent.RemoveListener(RefreshItemUI);
    }

    public void Reroll_Button()
    {
        GameManager.Instance.RerollItems();
    }

    public void WeaponUI_Init()
    {
        string curName = GameManager.Instance.GetJItems().weapon.itemName;
        int value = 0;
        for (int i = 0; i < weapon_dropdown.options.Count; i++)
        {
            if (weapon_dropdown.options[i].text == curName)
            {
                value = i;
            }
        }

        weapon_dropdown.value = value;
    }

    public void WeaponChange_Dropdown()
    {
        GameManager.Instance.ChangeWeaponItemName(weapon_dropdown.options[weapon_dropdown.value].text);
    }

    private StringBuilder sb = new StringBuilder();

    private void BuildTotalStatString()
    {
        sb.Clear();

        JItemsList itemsList = GameManager.Instance.GetJItems();
        float hpsum = 0, defsum = 0, movementspeedsum = 0, atksum = 0, atkspeedsum = 0, critratesum = 0;
        for (int i = 0; i < itemsList.items.Count; i++)
        {
            if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.HP))
            {
                hpsum += itemsList.items[i].itemOptions[ArtifactKey.HP];
            }

            if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.DEF))
            {
                defsum += itemsList.items[i].itemOptions[ArtifactKey.DEF];
            }

            if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.MOVEMENTSPEED))
            {
                movementspeedsum += itemsList.items[i].itemOptions[ArtifactKey.MOVEMENTSPEED];
            }

            if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.ATK))
            {
                atksum += itemsList.items[i].itemOptions[ArtifactKey.ATK];
            }

            if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.ATKSPEED))
            {
                atkspeedsum += itemsList.items[i].itemOptions[ArtifactKey.ATKSPEED];
            }

            if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.CRIT_RATE))
            {
                critratesum += itemsList.items[i].itemOptions[ArtifactKey.CRIT_RATE];
            }
        }

        hpsum = (float)Math.Round(hpsum, 2);
        defsum = (float)Math.Round(defsum, 2);
        movementspeedsum = (float)Math.Round(movementspeedsum, 2);
        atksum = (float)Math.Round(atksum, 2);
        atkspeedsum = (float)Math.Round(atkspeedsum, 2);
        critratesum = (float)Math.Round(critratesum, 2);

        sb.Append("체력 +");
        sb.Append(hpsum.ToString());
        sb.Append("\n");
        sb.Append("방어력 +");
        sb.Append(defsum.ToString());
        sb.Append("\n");
        sb.Append("이동속도 +");
        sb.Append(movementspeedsum.ToString());
        sb.Append("\n");
        sb.Append("공격력 +");
        sb.Append(atksum.ToString());
        sb.Append("\n");
        sb.Append("공격속도 +");
        sb.Append(atkspeedsum.ToString());
        sb.Append("\n");
        sb.Append("치명타 확률 +");
        sb.Append(critratesum.ToString());
        sb.Append("%");
        sb.Append("\n");

        statCombinationText.text = sb.ToString();
    }
    [Button]
    public void RefreshItemUI() // 아이템 정보 다시 읽어와서 디스플레이
    {
        JItemsList itemsList = GameManager.Instance.GetJItems();

        for (int i = 0; i < itemsList.items.Count; i++)
        {
            itemDisplays[i].GetChild(0).GetComponent<TMP_Text>().text = itemsList.items[i].itemName;
            itemDisplays[i].GetChild(1).GetComponent<TMP_Text>().text =
                "tier: " + itemsList.items[i].itemTier.ToString();

            sb.Clear();
            if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.HP))
            {
                sb.Append("체력 +");
                sb.Append(itemsList.items[i].itemOptions[ArtifactKey.HP].ToString());
                sb.Append("\n");
            }

            if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.DEF))
            {
                sb.Append("방어력 +");
                sb.Append(itemsList.items[i].itemOptions[ArtifactKey.DEF].ToString());
                sb.Append("\n");
            }

            if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.MOVEMENTSPEED))
            {
                sb.Append("이동속도 +");
                sb.Append(itemsList.items[i].itemOptions[ArtifactKey.MOVEMENTSPEED].ToString());
                sb.Append("\n");
            }

            if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.ATK))
            {
                sb.Append("공격력 +");
                sb.Append(itemsList.items[i].itemOptions[ArtifactKey.ATK].ToString());
                sb.Append("\n");
            }

            if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.ATKSPEED))
            {
                sb.Append("공격속도 +");
                sb.Append(itemsList.items[i].itemOptions[ArtifactKey.ATKSPEED].ToString());
                sb.Append("\n");
            }

            if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.CRIT_RATE))
            {
                sb.Append("치명타 확률 +");
                sb.Append(itemsList.items[i].itemOptions[ArtifactKey.CRIT_RATE].ToString());
                sb.Append("%");
                sb.Append("\n");
            }

            itemDisplays[i].GetChild(2).GetComponent<TMP_Text>().text = sb.ToString();
        }

        diceCountText.text = GameManager.Instance.GetCurrentDiceCount().ToString();
        BuildTotalStatString();
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }
}