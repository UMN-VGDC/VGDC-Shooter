using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

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

    private Renderer[] renderer;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponentsInChildren<Renderer>();
    }

    public void DecreaseHealth()
    {
        health--;
        if (health == 0) {
            isDead = true;
            DeathType();
            return;
        }
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
        if (isDead) return;
        for (int i = 0; i < renderer.Length; i++)
        {
            SetRenderColor(renderer[i], flashColor);
        }
        await UniTask.DelayFrame(10);
        // Second check if entity death type = disappear
        if (isDead) return;
        for (int i = 0; i < renderer.Length; i++)
        {
            SetRenderColor(renderer[i], new Color(1, 1, 1, 1));
        }
    }

    private void SetRenderColor(Renderer rend, Color color)
    {
        for (int i = 0; i < rend.materials.Length; i++)
        {
            rend.materials[i].SetColor("_Hit_Flash_1", color);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
