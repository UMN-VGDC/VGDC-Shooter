using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeManager : MonoBehaviour
{
    private bool isGunSelect;
    // Start is called before the first frame update
    void Start()
    {
        ScoreStreakManager.scoreStreak += StreakTimeWarp;
        MoneyManager.activateSwitch += MoneyFull;
        RandomizeGun.gunSelectGraphic += GunSelectGrpahic;
    }

    private void StreakTimeWarp() => TimeWarp(1f);
    private void MoneyFull() => TimeWarp(0.6f);

    private void GunSelectGrpahic()
    {
        isGunSelect = true;
        Sequence s = DOTween.Sequence();
        s.Append(DOVirtual.Float(1, 0.05f, 0.6f, e => Time.timeScale = e)).SetUpdate(true);
        s.Append(DOVirtual.Float(0.05f, 1, 0.6f, e => Time.timeScale = e).SetDelay(1f)).SetUpdate(true).OnComplete(() => isGunSelect = false);
    }

    private void TimeWarp(float duration)
    {
        DOVirtual.Float(0.1f, 1f, duration, e =>
        {
            if (isGunSelect) return;
            Time.timeScale = e;
        });
    }

    private void OnDestroy()
    {
        ScoreStreakManager.scoreStreak -= StreakTimeWarp;
        MoneyManager.activateSwitch -= MoneyFull;
        RandomizeGun.gunSelectGraphic += GunSelectGrpahic;
    }
}
