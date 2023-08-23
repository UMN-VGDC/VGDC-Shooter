using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Image healthBar, healthBarRed, lowHealthGlow;
    [SerializeField] private CanvasGroup glowGroup;
    [SerializeField] private int maxHealth = 3000;
    [SerializeField] private float regenerationSpeed = 0.01f;
    [SerializeField] private GameObject[] disableGUI;
    private float currentFill = 1f;
    private float currentHealth;
    private float lowHealthGlowOpacity;
    private bool isRegenerating = true;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.damageTakenAmount += LowerHealth;
        currentHealth = maxHealth;
        lowHealthGlow.DOColor(Color.black, 0.2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private void LowerHealth(int amount)
    {
        if (isDead) return;
        isRegenerating = false;
        float multiplier = amount;
        if (healthBar.fillAmount < 0.15f) multiplier = amount * 0.7f;
        currentHealth += multiplier;
        currentFill = currentHealth / maxHealth;        
        
        DOVirtual.Float(healthBar.fillAmount, currentFill, 0.5f, e =>
        {
            healthBar.fillAmount = e;
        }).SetEase(Ease.OutQuad).OnComplete(() => isRegenerating = true);

        if (currentFill <= 0f)
        {
            isDead = true;
            GameManager.Instance.UpdateGameState(GameState.Dead);
            currentFill = 0f;
            foreach(GameObject g in disableGUI) g.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        healthBarRed.fillAmount = Mathf.MoveTowards(healthBarRed.fillAmount, healthBar.fillAmount, Time.deltaTime * 0.13f);
        if (isRegenerating && healthBar.fillAmount != 1f)
        {
            float multiplier = healthBar.fillAmount < 0.15f ? 1.3f : 1;
            healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, 1f, Time.deltaTime * regenerationSpeed * multiplier);
            currentHealth = Mathf.Lerp(0, maxHealth, healthBar.fillAmount);
        }
        lowHealthGlowOpacity = healthBar.fillAmount < 0.3f ? 1 : 0;
        glowGroup.alpha = Mathf.MoveTowards(glowGroup.alpha, lowHealthGlowOpacity, Time.deltaTime * 2f);
    }

    private void OnDestroy()
    {
        PlayerHealth.damageTakenAmount -= LowerHealth;
        DOTween.Kill(lowHealthGlow);
    }
}
