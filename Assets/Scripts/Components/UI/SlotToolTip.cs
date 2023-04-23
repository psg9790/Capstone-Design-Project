using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    // 필요한 컴포넌트들
    [SerializeField] 
    private GameObject go_Base;
    [SerializeField] 
    private Text txt_Itemname;
    [SerializeField] 
    private Text txt_Itemdesc;

    
    static public SlotToolTip instance;

    private void Start()
    {
        instance = this;
    }

    public void ShowToolTip(Item item, Vector3 _pos )     //item  나중에 정호가 만든걸로 바꿔야됨.
    {
        go_Base.SetActive(true);
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,
            -go_Base.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        go_Base.transform.position = _pos;

        txt_Itemname.text = item.itemName;
        //txt_Itemdesc.text = item.itemDesc;
    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
