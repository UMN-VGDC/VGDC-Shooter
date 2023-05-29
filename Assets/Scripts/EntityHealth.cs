using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] private int health = 20;
    public enum EntityType
    {
        Enemy,
        Civilian,
        Prop
    }
    public EntityType entityType;


    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor = new Color(51, 0, 0, 1);
    [SerializeField] private int playerDamage = 1;

    public static Action enemyHit;

    private Renderer[] renderers;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void DecreaseHealth(int amount)
    {
        health -= amount;
        if (health <= 0) {
            isDead = true;
            DeathType();
            return;
        }
        enemyHit?.Invoke();
        HitFlash();
    }

    private void DeathType()
    {
        switch (entityType)
        {
            case EntityType.Enemy:
                GetComponent<EnemyDeath>().KillEnemy();
                break;
            case EntityType.Civilian:
                GetComponent<CivilianDeath>().KillCivilian();
                break;
            case EntityType.Prop:
                Debug.Log("prop destroyed!");
                Destroy(gameObject);
                break;
     
        }
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

        await UniTask.DelayFrame(10);
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

    public int GetPlayerDamage()
    {
        return playerDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
