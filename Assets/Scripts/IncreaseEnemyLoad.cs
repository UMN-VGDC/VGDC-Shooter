using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseEnemyLoad : MonoBehaviour
{
    [SerializeField] private int bossAmount;
    [SerializeField] private int mediumAmount;
    [SerializeField] private int smallAmount;
    [SerializeField] private int flyingAmount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Truck" || GameManager.Instance.getGameState() == GameState.Dead) return;
        Debug.Log("difficulty increased!");
        EnemyLoadCount.Instance.IncrementBossLoad(bossAmount);
        EnemyLoadCount.Instance.IncrementMediumLoad(mediumAmount);
        EnemyLoadCount.Instance.IncrementSmalLoad(smallAmount);
        EnemyLoadCount.Instance.IncrementFlyingLoad(flyingAmount);

    }
}
