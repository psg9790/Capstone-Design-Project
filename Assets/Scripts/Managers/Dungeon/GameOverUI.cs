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
        int before = GameManager.Instance.GetCurrentMaxLevel();
        curWorldLevel_text.text = GrowthLevelManager.Instance.worldLevel.ToString();
        diceEarned_text.text = GameManager.Instance.EndOfGrowthDungeon(GrowthLevelManager.Instance.worldLevel).ToString();
        int after = GameManager.Instance.GetCurrentMaxLevel();
        maxLevel_text.text = after.ToString();
        bool isHighScore = before < after;
        Sequence seq = DOTween.Sequence();
        seq.Append(backCG.DOFade(1,2.5f).From(0))
            .Append(gameOverTextCG.DOFade(1, 1f).From(0))
            .Append(growthResultCG.DOFade(1, 1f).From(0))
            .Append(returnToButtonCG.DOFade(1, 1f).From(0));
        if (isHighScore)
        {
            seq.Join(newMaxLevelCG.DOFade(1, 1f).From(0));
        }
    }

    [BoxGroup("Record")] public CanvasGroup recordResultCG;
    [BoxGroup("Record")] public TMP_Text curLevel_text;
    [BoxGroup("Record")] public TMP_Text record_text;
    [BoxGroup("Record")] public CanvasGroup newRecordCG;
    public void Display_RecordDungeonResult()
    {
        curLevel_text.text = RecordLevelManager.Instance.curLevel.ToString();
        bool isHighScore = GameManager.Instance.EndOfRecordDungeon(RecordLevelManager.Instance.curLevel);
        record_text.text = GameManager.Instance.GetCurrentRecord().ToString();
        Sequence seq = DOTween.Sequence();
        seq.Append(backCG.DOFade(1,2.5f).From(0))
            .Append(gameOverTextCG.DOFade(1, 1f).From(0))
            .Append(recordResultCG.DOFade(1, 1f).From(0))
            .Append(returnToButtonCG.DOFade(1, 1f).From(0));
        if (isHighScore)
        {
            seq.Join(newRecordCG.DOFade(1, 1f).From(0));
        }
    }

    public void ReturnTo()
    {
        SceneManager.LoadScene("Start_Map");
        Time.timeScale = 1;
    }
}
