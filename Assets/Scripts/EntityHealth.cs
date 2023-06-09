using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] private int health = 20;
    [SerializeField] private UnityEvent deathCallback;

    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor = new Color(51, 0, 0, 1);
    [SerializeField] private int playerDamage = 1;
    [SerializeField] private AudioClip[] deathSounds;

    public static Action enemyHit;
    public static Action<AudioClip[]> deathSound;

    private Renderer[] renderers;
    private bool isDead;
    private int healthDecrement;

    // Start is called before the first frame update
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        healthDecrement = health;
    }

    public void DecreaseHealth(int amount)
    {
        healthDecrement -= amount;
        if (healthDecrement <= 0) {
            isDead = true;
            deathCallback?.Invoke();
            deathSound?.Invoke(deathSounds);
            return;
        }
        enemyHit?.Invoke();
        HitFlash();
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

    public async void MultiHitFlash(int count)
    {
        for (int i = 0; i < count; i++)
        {
            for (int r = 0; r < renderers.Length; r++)
            {
                SetRenderColor(renderers[r], flashColor);
            }
            await Task.Delay(40);
            ResetRenderCol();
            await Task.Delay(40);
        }
    }

    public int GetPlayerDamage()
    {
        return playerDamage;
    }

    public void ResetHealth()
    {
        healthDecrement = health;
    }
}
