using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private int playerHealth = 20;

    public static event Action damageTaken;
    public static event Action<int> damageTakenAmount;
    public static event Action<int> totalHealth;

    // Start is called before the first frame update
    void Start()
    {
        totalHealth?.Invoke(playerHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        var root = other.gameObject.transform.root;
        if (root.tag == "Entity")
        {
            var getHealthComponent = root.GetComponent<EntityHealth>();
            getHealthComponent.DecreaseHealth(5);
            DecreaseHealth(getHealthComponent.GetPlayerDamage());
            damageTaken?.Invoke();
            damageTakenAmount?.Invoke(-getHealthComponent.GetPlayerDamage());
        }

        if (root.tag == "Enemy Bullet")
        {
            int bulletDamage = root.GetComponent<EnemyBullet>().GetPlayerDamage();
            damageTaken?.Invoke();
            damageTakenAmount?.Invoke(-bulletDamage);
            Destroy(root.gameObject);
        }
    }

    private void DecreaseHealth(int amount)
    {
        playerHealth -= amount;
        totalHealth?.Invoke(playerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
