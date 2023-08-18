using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScoreStreakManager.scoreStreak += StreakTimeWarp;
        MoneyManager.activateSwitch += MoneyFull;
    }

    private void StreakTimeWarp() => TimeWarp(1f);
    private void MoneyFull() => TimeWarp(0.6f);
    private void TimeWarp(float duration)
    {
        DOVirtual.Float(0.1f, 1f, duration, e =>
        {
            Time.timeScale = e;
        });
    }

    private void OnDestroy()
    {
        ScoreStreakManager.scoreStreak -= StreakTimeWarp;
        MoneyManager.activateSwitch -= MoneyFull;
    }
}
