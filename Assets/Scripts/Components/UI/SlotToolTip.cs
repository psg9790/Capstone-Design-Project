using System.Collections;
using System.Collections.Generic;
using System.Text;
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

    private StringBuilder sb = new StringBuilder();
    public void ShowToolTip(Item item, Vector3 _pos)     //item  나중에 정호가 만든걸로 바꿔야됨.
    {
        
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
/*
        if (pos.x < 0f) pos.x = 0f;

        if (pos.x > 1f) pos.x = 1f;

        if (pos.y < 0f) pos.y = 0f;

        if (pos.y > 1f) pos.y = 1f;
        


        transform.position = Camera.main.ViewportToWorldPoint(pos);
    */

        go_Base.SetActive(true);
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f ,
            -go_Base.GetComponent<RectTransform>().rect.height * 0.5f+  tooltip.GetComponent<RectTransform>().rect.height , 0f);
        if (pos.y<_pos.y )
        {
            UnityEngine.Debug.Log("asdkfhgiwerhgiuwehgirhgkjdfgkj");
                /*
            _pos.y = -go_Base.GetComponent<RectTransform>().rect.height * 0.5f +
                     -2 * (tooltip.GetComponent<RectTransform>().rect.height);
                     */
        }

        go_Base.transform.position = _pos;
        
        List<string> stats= item.Options_ToString();

        txt_Itemname.text = item.itemName;
        txt_Itemdesc.text = item.itemData.tooltip;

        // if ((item.itemData.itemType==ItemType.Weapon)|| (item.itemData.itemType==ItemType.Artifact))
        {
            sb.Clear();
            for (int i = 0; i < stats.Count; i++)
            {
                sb.Append(stats[i]);
                sb.Append("\n");
                // if (i == 0)
                // {
                //     txt_Itemstat.text = stats[i];
                //     txt_Itemstat.text += "\n";
                // }
                // else
                // {
                //     txt_Itemstat.text += stats[i];
                //     txt_Itemstat.text += "\n";
                // }
            }

            txt_Itemstat.text = sb.ToString();
        }
        
    }

    public void HideToolTip()
    {
        
        go_Base.SetActive(false);
    }
}
