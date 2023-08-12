using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject[] enemy;
    [SerializeField] private int spawnRate = 5000;
    [SerializeField] private bool continuousSpawn;
    [SerializeField] private int amount = 3;
    private int defaultAmount;

    public static Action bossSpawned;

    // Start is called before the first frame update
    void Start()
    {
        if (continuousSpawn)
        {
            BeginSpawn();
        }

        defaultAmount = amount;

    }
    public async void SpawnFromTrigger()
    {
        if (amount == 0)
        {
            amount = defaultAmount;
            return;
        }
        int random = UnityEngine.Random.Range(0, enemy.Length);

        //Do not instantiate if max load is exceeded
        EntityHealth getEnemyData = enemy[random].GetComponent<EntityHealth>();
        int getLoad = getEnemyData.enemyLoad;
        string getType = getEnemyData.enemyLoadType.ToString();
        if (!EnemyLoadCount.Instance.ModifyLoad(-getLoad, getType)) return;
        if (getType == "Boss") bossSpawned?.Invoke();

        Instantiate(enemy[random], transform.position, Quaternion.identity);
        await Task.Delay(spawnRate);
        amount--;
        SpawnFromTrigger();
    }
    private async void BeginSpawn()
    {
        await Task.Delay(spawnRate);
        int random = UnityEngine.Random.Range(0, enemy.Length);
        Instantiate(enemy[random], transform.position, Quaternion.identity);
        BeginSpawn();
    }
}
