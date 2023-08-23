using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeManager : MonoBehaviour
{
    private bool isGunSelect, isDead;
    // Start is called before the first frame update
    void Start()
    {
        ScoreStreakManager.scoreStreak += StreakTimeWarp;
        MoneyManager.activateSwitch += MoneyFull;
        RandomizeGun.gunSelectGraphic += GunSelectGrpahic;
        GameManager.hasDied += DeathTimeWarp;
    }

    private void DeathTimeWarp()
    {
        isDead = true;
        DOVirtual.Float(0.7f, 0.02f, 8f, e =>
        {
            Time.timeScale = e;
        }).SetUpdate(true);
    }

    private void StreakTimeWarp() => TimeWarp(1f);
    private void MoneyFull() => TimeWarp(0.6f);

    private void GunSelectGrpahic()
    {
        if (isDead) return;
        isGunSelect = true;
        Sequence s = DOTween.Sequence();
        s.Append(DOVirtual.Float(1, 0.05f, 0.6f, e => Time.timeScale = e)).SetUpdate(true);
        s.Append(DOVirtual.Float(0.05f, 1, 0.6f, e => Time.timeScale = e).SetDelay(1f)).SetUpdate(true).OnComplete(() => isGunSelect = false);
    }

    private void TimeWarp(float duration)
    {
        DOVirtual.Float(0.1f, 1f, duration, e =>
        {
            if (isGunSelect || isDead) return;
            Time.timeScale = e;
        });
    }

    private void OnDestroy()
    {
        ScoreStreakManager.scoreStreak -= StreakTimeWarp;
        MoneyManager.activateSwitch -= MoneyFull;
        RandomizeGun.gunSelectGraphic += GunSelectGrpahic;
        GameManager.hasDied += DeathTimeWarp;
    }
}
