using System;
using System.Collections;
using System.Collections.Generic;
using Stove.PCSDK.NET;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private GameObject leaderBoard_panel;
    [SerializeField] private Button showgrowth_button;
    // [SerializeField] private GameObject growth_panel;
    [SerializeField] private Button showrecord_button;
    // [SerializeField] private GameObject record_panel;

    public RectTransform content;
    public RankComponent prefab;

    public RankComponent playerProfile;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void LeaderBoard_ButtonClick()
    {
#if StovePCSDK
        leaderBoard_panel.SetActive(true);
        Button_ShowRecord();
#else
    StovePCSDKManager.Instance.WriteLog("Stove 버전이 아닙니다.");
#endif
    }

    public void Button_ShowGrowth()
    {
        // growth_panel.SetActive(true);
        // record_panel.SetActive(false);
        showgrowth_button.GetComponentInChildren<TMP_Text>().color = new Color(0.9f, 0.9f, 0.4f);
        showrecord_button.GetComponentInChildren<TMP_Text>().color = new Color(0.9f, 0.9f, 0.9f);
        StovePCSDKManager.Instance.OnRankEvent.AddListener(RankingListener);
        StovePCSDKManager.Instance.GetRankMethod("NTIMES_IND_DEMO_01_IND|GROWTH_LEVEL", 
            1, 100, true);
    }

    public void Button_ShowRecord()
    {
        // growth_panel.SetActive(false);
        // record_panel.SetActive(true);
        showgrowth_button.GetComponentInChildren<TMP_Text>().color = new Color(0.9f, 0.9f, 0.9f);
        showrecord_button.GetComponentInChildren<TMP_Text>().color = new Color(0.9f, 0.9f, 0.4f);
        StovePCSDKManager.Instance.OnRankEvent.AddListener(RankingListener);
        StovePCSDKManager.Instance.GetRankMethod("NTIMES_IND_DEMO_01_IND|RECORD_LEVEL", 
        1, 100, true);
    }

    public void RankingListener(StovePCRank[] ranks, uint rankTotalCount)
    {
        RankComponent[] components = content.GetComponentsInChildren<RankComponent>();
        for (int i = 0; i < components.Length; i++)
        {
            Destroy(components[i].gameObject);
        }

        playerProfile.rank.text = ranks[0].Rank.ToString() + "위";
        playerProfile.userName.text = ranks[0].Nickname;
        playerProfile.level.text = ranks[0].Score.ToString() + "층";
        StartCoroutine(GetTexture(playerProfile.profile, ranks[0].ProfileImage));

        Vector2 wh = content.sizeDelta;
        wh.y = 100 * ranks.Length;
        content.sizeDelta = wh;
        for (int i = 1; i < ranks.Length; i++)
        {
            RankComponent component = Instantiate(prefab, content);
            component.rank.text = (ranks[i].Rank).ToString() + "위";
            component.userName.text = ranks[i].Nickname;
            component.level.text = ranks[i].Score.ToString() + "층";
            if (ranks[i].MemberNo == StovePCSDKManager.Instance.LOGIN_USER_MEMBER_NO)
            {
                component.GetComponentInChildren<Image>().color = Color.red;
            }

            StartCoroutine(GetTexture(component.profile, ranks[i].ProfileImage));

            RectTransform rect = component.GetComponent<RectTransform>();
            Vector3 anc = rect.anchoredPosition3D;
            anc.y = -100 * (i - 1);
            rect.anchoredPosition3D = anc;
        }
        
        StovePCSDKManager.Instance.OnRankEvent.RemoveListener(RankingListener);
    }

    IEnumerator GetTexture(RawImage rawImage, string path)
    {
        var url = path;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            rawImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

    public void Close_LeaderBoard()
    {
        leaderBoard_panel.SetActive(false);
    }
}