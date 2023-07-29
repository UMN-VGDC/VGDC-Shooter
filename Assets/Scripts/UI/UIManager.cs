using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Threading.Tasks;

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

    private int currentStreak;

    public static Action scoreStreak;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.damageTaken += spawnBloodScratch;
        PlayerHealth.damageTakenAmount += AddPoints;
        EnemyDeath.killPoints += AddPoints;
        currentStreak = streakCount;
        defaultStreakColor = score.color;
        defaultOutlineColor = score.outlineColor;

    }

    private int currentScore;
    private bool scoreCanKill = true;
    private async void AddPoints(int amount)
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        PlayerHealth.damageTaken -= spawnBloodScratch;
        PlayerHealth.damageTakenAmount -= AddPoints;
        EnemyDeath.killPoints -= AddPoints;
    }
}
