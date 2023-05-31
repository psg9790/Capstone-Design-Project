using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SlotToolTip : MonoBehaviour
{
    // 필요한 컴포넌트들
    [SerializeField] private GameObject go_Base;
    public TMP_Text txt_Itemname;
    public TMP_Text txt_Itemdesc;
    public TMP_Text txt_Itemstat;
    [SerializeField] private GameObject tooltip;

    private int resolution = Screen.height;

    private StringBuilder sb = new StringBuilder();

    public void ShowToolTip(Item item, Vector3 _pos) //item  나중에 정호가 만든걸로 바꿔야됨.
    {
        
        go_Base.SetActive(true);
        RectTransform rect = GetComponent<RectTransform>();
        RectTransform baseRect = go_Base.GetComponent<RectTransform>();
        
        Resolution res = Screen.currentResolution;
        Vector2 mousePosition = InputManager.Instance.GetMousePosition();
        rect.anchoredPosition3D =
            new Vector3(mousePosition.x - res.width * 0.5f, mousePosition.y - res.height * 0.5f, 0);
        baseRect.anchoredPosition3D = new Vector3(300, ((mousePosition.y > 500) ? -250 : 250), 0);

        List<string> stats = item.Options_ToString();

        txt_Itemname.text = item.itemName;
        txt_Itemdesc.text = item.itemData.tooltip;

        sb.Clear();
        for (int i = 0; i < stats.Count; i++)
        {
            sb.Append(stats[i]);
            sb.Append("\n");
        }

        txt_Itemstat.text = sb.ToString();
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}