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

        RectTransform rect = go_Base.GetComponent<RectTransform>();
        Vector2 mouse =  InputManager.Instance.GetMousePosition();
        rect.transform.position = mouse;
        mouse = new Vector2(mouse.x - Screen.width * 0.5f, mouse.y - Screen.height * 0.5f);
        if (mouse.y <= 0)
        {
            if (mouse.x <= 0)
            {
                rect.pivot = new Vector2(0, 0);
            }
            else
            {
                rect.pivot = new Vector2(1, 0);
            }
        }
        else
        {
            if (mouse.x <= 0)
            {
                rect.pivot = new Vector2(0, 1);
            }
            else
            {
                rect.pivot = new Vector2(1, 1);
            }
        }
        // RectTransform parent = GetComponent<RectTransform>();
        // RectTransform rect = go_Base.GetComponent<RectTransform>();
        // if (parent.anchoredPosition.x + rect.sizeDelta.x > 960)
        // {
        //     rect.pivot = new Vector2(1, 1);
        // }
        // else
        // {
        //     rect.pivot = new Vector2(0, 1);
        // }
        
        //
        // RectTransform rect = GetComponent<RectTransform>();
        // RectTransform baseRect = go_Base.GetComponent<RectTransform>();
        //
        // Resolution res = Screen.currentResolution;
        // Vector2 mousePosition = InputManager.Instance.GetMousePosition();
        // float scaleX = mousePosition.x / res.width, scaleY = mousePosition.y / res.height;
        //
        //
        // rect.anchoredPosition3D =
        //     new Vector3(1920 * scaleX - 960, 1080 * scaleY - 540, 0);
        // baseRect.anchoredPosition3D = new Vector3(baseRect.sizeDelta.x * 0.5f, ((mousePosition.y > baseRect.sizeDelta.y) ? -baseRect.sizeDelta.y * 0.5f : baseRect.sizeDelta.y * 0.5f), 0);
        //
        //
        
        
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