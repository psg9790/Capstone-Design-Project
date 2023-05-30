using System.Collections;
using System.Collections.Generic;
using System.Resources;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public CanvasGroup backCG;
    public CanvasGroup gameOverTextCG;
    public CanvasGroup returnToButtonCG;
    
    [BoxGroup("Growth")] public CanvasGroup growthResultCG;
    [BoxGroup("Growth")] public TMP_Text curWorldLevel_text;
    [BoxGroup("Growth")] public TMP_Text diceEarned_text;
    [BoxGroup("Growth")] public TMP_Text maxLevel_text;
    [BoxGroup("Growth")] public CanvasGroup newMaxLevelCG;

    public void Display_GrowthDungeonResult()
    {
        int before = GameManager.Instance.Save.GetGrowthLevel();
        GameManager.Instance.Save.SetGrowthLevel(GrowthLevelManager.Instance.worldLevel);

        Sequence seq = DOTween.Sequence();
        seq.Append(backCG.DOFade(1,2.5f).From(0))
            .Append(gameOverTextCG.DOFade(1, 1f).From(0))
            .Join(DOVirtual.DelayedCall(0.1f,()=> SetGrowthText(before)))
            .Append(growthResultCG.DOFade(1, 1f).From(0))
            .Append(returnToButtonCG.DOFade(1, 1f).From(0));
    }

    void SetGrowthText(int before)
    {
        curWorldLevel_text.text = GrowthLevelManager.Instance.worldLevel.ToString();
        
        int earnedCoin = (int)(GrowthLevelManager.Instance.worldLevel * 1.5f);
        GameManager.Instance.AddCoin(earnedCoin);
        diceEarned_text.text = earnedCoin.ToString();
        
        int after = GameManager.Instance.Save.GetGrowthLevel();
        maxLevel_text.text = after.ToString();
        
        bool isHighScore = before < after;
        if(isHighScore)
            newMaxLevelCG.DOFade(1, 1f).From(0);
    }

    [BoxGroup("Record")] public CanvasGroup recordResultCG;
    [BoxGroup("Record")] public TMP_Text curLevel_text;
    [BoxGroup("Record")] public TMP_Text record_text;
    [BoxGroup("Record")] public CanvasGroup newRecordCG;
    public void Display_RecordDungeonResult()
    {
        int before = GameManager.Instance.Save.GetRecordLevel();
        Debug.Log(before);
        GameManager.Instance.Save.SetRecordLevel(RecordLevelManager.Instance.curLevel);
        
       
        Sequence seq = DOTween.Sequence();
        seq.Append(backCG.DOFade(1,2.5f).From(0))
            .Append(gameOverTextCG.DOFade(1, 1f).From(0))
            .Join(DOVirtual.DelayedCall(0.1f,() => SetRecordText(before)))
            .Append(recordResultCG.DOFade(1, 1f).From(0))
            .Append(returnToButtonCG.DOFade(1, 1f).From(0));
        
    }

    void SetRecordText(int before)
    {
        curLevel_text.text = RecordLevelManager.Instance.curLevel.ToString();
        int after = GameManager.Instance.Save.GetRecordLevel();
        
        Debug.Log(before.ToString() + " " + after.ToString()); // 2 4
        bool isHighScore = before < after;
        record_text.text = after.ToString();
        if(isHighScore)
            newRecordCG.DOFade(1, 1f).From(0);
    }

    public void ReturnTo()
    {
        SceneManager.LoadScene("Start_Map");
        Time.timeScale = 1;
    }
}
