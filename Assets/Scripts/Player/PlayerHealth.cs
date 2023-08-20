using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action damageTaken;
    public static event Action<int> damageTakenAmount;
    private void OnTriggerEnter(Collider other)
    {
        var root = other.gameObject.transform.root;
        if (root.tag == "Entity")
        {
            var getHealthComponent = root.GetComponent<EntityHealth>();
            getHealthComponent.DecreaseHealth(5);
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
}
