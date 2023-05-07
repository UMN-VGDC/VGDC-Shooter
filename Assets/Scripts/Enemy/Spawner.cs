using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject[] enemy;
    [SerializeField] private int spawnRate = 5000;
    // Start is called before the first frame update
    void Start()
    {
        BeginSpawn();

    }

    private async void BeginSpawn()
    {
        await Task.Delay(spawnRate);
        int random = Random.Range(0, enemy.Length);
        Instantiate(enemy[random], transform.position, Quaternion.identity);
        BeginSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
