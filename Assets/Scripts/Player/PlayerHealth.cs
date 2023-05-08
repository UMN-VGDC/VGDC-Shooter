using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private int playerHealth = 20;

    public static Action damageTaken;
    public static Action<int> damageTakenAmount;
    public static Action<int> totalHealth;

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
            damageTakenAmount?.Invoke(getHealthComponent.GetPlayerDamage());
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
