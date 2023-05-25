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
    
    private void Start()
    {
        GameManager.Instance.itemChangedEvent.AddListener(RefreshItemUI);
    }

    private void OnDestroy()
    {
        GameManager.Instance.itemChangedEvent.RemoveListener(RefreshItemUI);
    }

    public void Reroll_Button()
    {
        GameManager.Instance.RerollItems();
    }

    private StringBuilder sb = new StringBuilder();
    [Button]
    public void RefreshItemUI() // 아이템 정보 다시 읽어와서 디스플레이
    {
        JItemsList itemsList = GameManager.Instance.GetJItems();

        for (int i = 0; i < itemsList.items.Count; i++)
        {
            itemDisplays[i].GetChild(0).GetComponent<TMP_Text>().text = itemsList.items[i].itemName;
            itemDisplays[i].GetChild(1).GetComponent<TMP_Text>().text = "tier: " + itemsList.items[i].itemTier.ToString();
            sb.Clear();
            for (int j = 0; j < itemsList.items[i].itemOptions.Count; j++)
            {
                if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.HP))
                {
                    sb.Clear();
                    sb.Append("체력 +");
                    sb.Append(itemsList.items[i].itemOptions[ArtifactKey.HP].ToString());
                    sb.Append("\n");
                }

                if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.DEF))
                {
                    sb.Clear();
                    sb.Append("방어력 +");
                    sb.Append(itemsList.items[i].itemOptions[ArtifactKey.DEF].ToString());
                    sb.Append("\n");
                }

                if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.MOVEMENTSPEED))
                {
                    sb.Clear();
                    sb.Append("이동속도 +");
                    sb.Append(itemsList.items[i].itemOptions[ArtifactKey.MOVEMENTSPEED].ToString());
                    sb.Append("\n");
                }

                if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.ATK))
                {
                    sb.Clear();
                    sb.Append("공격력 +");
                    sb.Append(itemsList.items[i].itemOptions[ArtifactKey.ATK].ToString());
                    sb.Append("\n");
                }

                if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.ATKSPEED))
                {
                    sb.Clear();
                    sb.Append("공격속도 +");
                    sb.Append(itemsList.items[i].itemOptions[ArtifactKey.ATKSPEED].ToString());
                    sb.Append("\n");
                }

                if (itemsList.items[i].itemOptions.ContainsKey(ArtifactKey.CRIT_RATE))
                {
                    sb.Clear();
                    sb.Append("치명타 확률 +");
                    sb.Append(itemsList.items[i].itemOptions[ArtifactKey.CRIT_RATE].ToString());
                    sb.Append("%");
                    sb.Append("\n");
                }
            }
            itemDisplays[i].GetChild(2).GetComponent<TMP_Text>().text = sb.ToString();
        }

        diceCountText.text = GameManager.Instance.GetCurrentDiceCount().ToString();
    }
}