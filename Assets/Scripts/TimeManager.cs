using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.scoreStreak += StreakTimeWarp;
    }

    private void StreakTimeWarp()
    {
        DOVirtual.Float(0.1f, 1f, 1f, e =>
        {
            Time.timeScale = e;
        });
    }

    private void OnDestroy()
    {
        UIManager.scoreStreak -= StreakTimeWarp;
    }
}
