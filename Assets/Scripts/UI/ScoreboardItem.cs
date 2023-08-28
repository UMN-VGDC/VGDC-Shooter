using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardItem : MonoBehaviour
{
    public TextMeshProUGUI rank, playerName, score;
    public CanvasGroup backgroundimage;
    private bool isActive;
    public static Action stopScrolling;

    public void HighlightName()
    {
        playerName.DOColor(Color.green, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        score.DOColor(Color.green, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        isActive = true;
    }

    public void Update()
    {
        if (!isActive) return;
        if (transform.position.y > 325)
        {
            isActive = false;
            stopScrolling?.Invoke();
        }
    }

}
