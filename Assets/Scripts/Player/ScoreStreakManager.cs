using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class ScoreStreakManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private int streakCount = 5000;
    [SerializeField] private Color32 streakColor, streakOutlineColor;
    private Color32 defaultStreakColor, defaultOutlineColor;

    [SerializeField] private Transform scoreAddPosition;
    [SerializeField] private TextMeshProUGUI scoreAdd;
    [SerializeField] private CanvasRenderer speedLinesRenderer;
    [SerializeField] private Image scoreStreakBar, scoreStreakBarGreen;
    private float barFillSpeed = 0.13f;

    private int currentStreak, comboCount;

    public static Action scoreStreak;
    public static ScoreStreakManager Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.damageTakenAmount += AddPoints;
        Bullet.killPoints += AddPoints;
        FlamethrowerHurtbox.killPoints += AddPoints;
        SoundManager.flameThrower += SpeedLinesEffect;
        Bullet.crit += CritPoints;
        MultiTargetEnemy.multiTargetsPoints += AddPoints;

        currentStreak = streakCount;
        defaultStreakColor = score.color;
        defaultOutlineColor = score.outlineColor;
        Instance = this;
    }

    private async void CritPoints()
    {
        AddPoints(comboCount);
        comboCount++;
        int currentCombo = comboCount;
        await Task.Delay(500);
        if (currentCombo == comboCount)
        {
            comboCount = 0;
        }
    }

    private int currentScore;
    private bool scoreCanKill = true;
    private async void AddPoints(int amount)
    {
        if (amount == 0) return;
        currentScore += amount;
        scoreStreakBarGreen.fillAmount = (currentScore - (currentStreak - streakCount)) / (float)streakCount;

        float scaleAmount = 1.5f;
        if (currentScore >= currentStreak)
        {
            score.color = streakColor;
            score.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, streakOutlineColor);
            scaleAmount = 3f;
            scoreStreakBar.fillAmount = 1;
            DOVirtual.Color(streakColor, Color.white, 1f, e =>
            {
                scoreStreakBar.color = e;
            });
            barFillSpeed = 2;
        }

        score.text = currentScore.ToString();
        if (scoreCanKill) DOTween.Kill(score.transform);
        score.transform.localScale = new Vector3(scaleAmount, scaleAmount, scaleAmount);
        score.transform.DOScale(1, 0.5f);

        DOVirtual.Color(score.color, defaultStreakColor, 0.5f, e =>
        {
            score.color = e;
        });
        DOVirtual.Color(score.outlineColor, defaultOutlineColor, 0.5f, e =>
        {
            score.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, e);
        });

        if (currentScore >= currentStreak)
        {
            scoreStreak?.Invoke();
            currentStreak += streakCount;
            scoreCanKill = false;
            await Task.Delay(700);
            scoreCanKill = true;
        }

        //Add Score Effect
        GameObject add = Instantiate(scoreAdd.gameObject, scoreAddPosition.position, Quaternion.identity, scoreAddPosition);
        add.GetComponent<ScoreAddEffect>().SetText(amount);
    }

    private void Update()
    {
        if (scoreCanKill)
        {
            scoreStreakBar.fillAmount = Mathf.MoveTowards(scoreStreakBar.fillAmount, scoreStreakBarGreen.fillAmount, Time.deltaTime * barFillSpeed);
            barFillSpeed = Mathf.MoveTowards(barFillSpeed, 0.13f, Time.deltaTime);
        }
    }

    private async void SpeedLinesEffect()
    {
        DOVirtual.Float(0, 1, 0.5f, e =>
        {
            speedLinesRenderer.GetMaterial().SetFloat("_Opacity", e);
        });
        await Task.Delay(3000);
        DOVirtual.Float(1, 0, 0.5f, e =>
        {
            speedLinesRenderer.GetMaterial().SetFloat("_Opacity", e);
        });
    }

    public void ResetEffects()
    {
        speedLinesRenderer.GetMaterial().SetFloat("_Opacity", 0);
    }

    private void OnDestroy()
    {
        PlayerHealth.damageTakenAmount -= AddPoints;
        Bullet.killPoints -= AddPoints;
        FlamethrowerHurtbox.killPoints -= AddPoints;
        SoundManager.flameThrower -= SpeedLinesEffect;
        Bullet.crit -= CritPoints;
        MultiTargetEnemy.multiTargetsPoints -= AddPoints;
    }
}
