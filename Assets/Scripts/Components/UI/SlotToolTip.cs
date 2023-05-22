using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    // 필요한 컴포넌트들
    [SerializeField] 
    private GameObject go_Base;
    public TMP_Text txt_Itemname;
    public TMP_Text txt_Itemdesc;
    public TMP_Text txt_Itemstat;
    [SerializeField]
    private GameObject tooltip;
    public void ShowToolTip(Item item, Vector3 _pos)     //item  나중에 정호가 만든걸로 바꿔야됨.
    {
        go_Base.SetActive(true);
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f ,
            -go_Base.GetComponent<RectTransform>().rect.height * 0.5f+  tooltip.GetComponent<RectTransform>().rect.height , 0f);
        go_Base.transform.position = _pos;
        
        List<string> stats= item.Options_ToString();

        txt_Itemname.text = item.itemName;
        txt_Itemdesc.text = item.itemData.tooltip;

        if ((item.itemData.itemType==ItemType.Weapon)|| (item.itemData.itemType==ItemType.Artifact))
        {
            for (int i = 0; stats[i] != null; i++)
            {
                if (i == 0)
                {
                    txt_Itemstat.text = stats[i];
                    txt_Itemstat.text += "\n";
                }
                else
                {
                    txt_Itemstat.text += stats[i];
                    txt_Itemstat.text += "\n";
                }
            }
        }
        
    }

    public void HideToolTip()
    {
        
        go_Base.SetActive(false);
    }
}
