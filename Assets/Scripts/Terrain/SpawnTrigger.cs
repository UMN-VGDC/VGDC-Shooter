using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private int id = 0;
    public static Action<int> triggerSpawn;
    private Spawner[] spawners;

    // Start is called before the first frame update
    void Start()
    {
        spawners = GetComponentsInChildren<Spawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Truck" || GameManager.Instance.getGameState() == GameState.Dead) return;
        triggerSpawn?.Invoke(id);
        foreach (Spawner spawner in spawners)
        {
            spawner.SpawnFromTrigger();
        }
    }

}
