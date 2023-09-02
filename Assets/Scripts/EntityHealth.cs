using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.Events;
using DG.Tweening;
using System.Numerics;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] private int health = 20;
    [SerializeField] private UnityEvent deathCallback;

    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor = new Color(51, 0, 0, 1);
    [SerializeField] private int playerDamage = 1;
    [SerializeField] private int points = 100;

    public static Action enemyHit;
    public static Action<int> enemyDeath;

    private Renderer[] renderers;
    private bool isDead;
    private int healthDecrement;

    public enum EnemyLoadType
    {
        Small,
        Medium,
        Boss,
        Flying
    }
    public EnemyLoadType enemyLoadType;
    public int enemyLoad;

    // Start is called before the first frame update
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        healthDecrement = health;
    }

    public int DecreaseHealth(int amount)
    {
        healthDecrement -= amount;
        if (healthDecrement <= 0) {
            isDead = true;
            deathCallback?.Invoke();
            enemyDeath?.Invoke(points);
            EnemyLoadCount.Instance.ModifyLoad(enemyLoad, enemyLoadType.ToString());
            return points;
        }
        enemyHit?.Invoke();
        HitFlash();
        return 0;
    }

    public void AddLoad()
    {
        if (isDead) return;
        EnemyLoadCount.Instance.ModifyLoad(enemyLoad, enemyLoadType.ToString());
    }

    private async void HitFlash()
    {
        if (isDead) {
            ResetRenderCol();
            return;
        }

        for (int i = 0; i < renderers.Length; i++) {
            SetRenderColor(renderers[i], flashColor);
        }

        await Task.Delay(10);
        ResetRenderCol();
    }

    private void ResetRenderCol()
    {
        for (int i = 0; i < renderers.Length; i++) {
            SetRenderColor(renderers[i], new Color(1, 1, 1, 1));
        }
    }

    private void SetRenderColor(Renderer rend, Color color)
    {
        if(rend == null)
        {
            return;
        }
        for (int i = 0; i < rend.materials.Length; i++)
        {
            rend.materials[i].SetColor("_Hit_Flash_1", color);
        }
    }

    public async void MultiHitFlash(int count, float fresnelAmount = 1f, bool isFresnel = false)
    {
        for (int i = 0; i < count; i++)
        {
            for (int r = 0; r < renderers.Length; r++)
            {
                SetRenderColor(renderers[r], flashColor);
            }
            if (isFresnel) SetFresnel(fresnelAmount);
            await Task.Delay(40);
            ResetRenderCol();
            if (isFresnel) SetFresnel(0);
            await Task.Delay(40);
        }

        void SetFresnel(float factor)
        {
            for (int r = 0; r < renderers.Length; r++)
            {
                SetFresnelFactor(renderers[r], factor);
            }
        }
    }

    private void SetFresnelFactor(Renderer rend, float factor)
    {
        if (rend == null)
        {
            return;
        }
        for (int i = 0; i < rend.materials.Length; i++)
        {
            rend.materials[i].SetFloat("_Buff_Factor", factor);
        }
    }

    public void BossWeakenEffect(float duration)
    {
        DOVirtual.Float(1f, 0f, duration, e =>
        {
            for (int r = 0; r < renderers.Length; r++)
            {
                SetFresnelFactor(renderers[r], e);
            }
        });
    }

    public int GetPlayerDamage()
    {
        return playerDamage;
    }

    public int GetCurrentHealth()
    {
        return healthDecrement;
    }

    public void ResetHealth()
    {
        healthDecrement = health;
    }
}
