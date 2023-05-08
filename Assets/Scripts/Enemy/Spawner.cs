using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject[] enemy;
    [SerializeField] private int spawnRate = 5000;
    [SerializeField] private bool continuousSpawn;
    [SerializeField] private int amount = 3;
    private int defaultAmount;
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
        int random = Random.Range(0, enemy.Length);
        Instantiate(enemy[random], transform.position, Quaternion.identity);
        await Task.Delay(spawnRate);
        amount--;
        SpawnFromTrigger();
    }
    private async void BeginSpawn()
    {
        await Task.Delay(spawnRate);
        int random = Random.Range(0, enemy.Length);
        Instantiate(enemy[random], transform.position, Quaternion.identity);
        BeginSpawn();
    }
}
