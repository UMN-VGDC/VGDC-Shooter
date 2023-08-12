using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEditor;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject[] bloodScratch;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private int streakCount = 5000;
    [SerializeField] private Color32 streakColor, streakOutlineColor;
    private Color32 defaultStreakColor, defaultOutlineColor;

    [SerializeField] private Transform scoreAddPosition;
    [SerializeField] private TextMeshProUGUI scoreAdd;
    [SerializeField] private CanvasRenderer speedLinesRenderer;

    [Header("Warning Effect")]
    [SerializeField] private CanvasGroup warningGroup;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private AudioClip warningBossSound;
    private bool warningIsPlaying;

    private bool isDestroyed;
    private int currentStreak, comboCount;

    public static Action scoreStreak;
    public static Action<AudioClip> warningSound;
    public static UIManager Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.damageTaken += spawnBloodScratch;
        PlayerHealth.damageTakenAmount += AddPoints;
        Bullet.killPoints += AddPoints;
        SoundManager.flameThrower += SpeedLinesEffect;
        Bullet.crit += CritPoints;
        MultiTargetEnemy.multiTargetsPoints += AddPoints;
        Spawner.bossSpawned += BossWarningEffect;

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
        await Task.Delay (500);
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
        float scaleAmount = 1.5f;
        if (currentScore >= currentStreak)
        {
            score.color = streakColor;
            score.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, streakOutlineColor);
            scaleAmount = 3f;
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

    private async void BossWarningEffect()
    {
        if (warningIsPlaying) return;
        warningIsPlaying = true;
        warningSound?.Invoke(warningBossSound);

        DOVirtual.Float(0, 1, 0.5f, e =>
        {
            warningGroup.alpha = e;
        });

        DOVirtual.Float(50, 90, 4f, e =>
        {
            warningText.characterSpacing = e;
        });

        await Task.Delay(3000);
        if (isDestroyed) return;
        DOVirtual.Float(1, 0, 0.5f, e =>
        {
            warningGroup.alpha = e;
        });

        await Task.Delay(3000);
        if (isDestroyed) return;
        warningIsPlaying = false;
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

    private void spawnBloodScratch()
    {
        Vector2 ramdomPos = new Vector2(UnityEngine.Random.Range(-200f, 200f), UnityEngine.Random.Range(-150f, 50f));
        GameObject decal = bloodScratch[UnityEngine.Random.Range(0, bloodScratch.Length)];
        GameObject instantiatedDecal = Instantiate(decal, Vector2.zero, Quaternion.identity, canvas.transform);
        RectTransform rt = instantiatedDecal.GetComponent<RectTransform>();
        rt.anchoredPosition = rt.transform.position;
        rt.localPosition = ramdomPos;

        int flip = UnityEngine.Random.value < 0.5f ? 1 : -1;
        Vector2 initialScale = instantiatedDecal.transform.localScale;
        instantiatedDecal.transform.localScale = new Vector2(initialScale.x * flip, initialScale.y);
    }

    private void OnDestroy()
    {
        PlayerHealth.damageTaken -= spawnBloodScratch;
        PlayerHealth.damageTakenAmount -= AddPoints;
        Bullet.killPoints -= AddPoints;
        SoundManager.flameThrower -= SpeedLinesEffect;
        Bullet.crit -= CritPoints;
        MultiTargetEnemy.multiTargetsPoints -= AddPoints;
        Spawner.bossSpawned -= BossWarningEffect;
        isDestroyed = true;
    }
}
