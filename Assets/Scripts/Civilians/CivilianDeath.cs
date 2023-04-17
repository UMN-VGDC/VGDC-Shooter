using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianDeath : MonoBehaviour
{
    [SerializeField] private GameObject[] DeathSpawn;
    [SerializeField] private float spawnYOffset;
    [SerializeField] private GameObject[] hideObjects;

    private Rigidbody[] deathRB;
    private FlickerDisappear flickerDisappear;

    void Start()
    {
        deathRB = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < deathRB.Length; i++) 
        {
            deathRB[i].isKinematic = true;
        }

        flickerDisappear = GetComponent<FlickerDisappear>();
    }

    public void KillCivilian()
    {
        for (int i = 0; i < deathRB.Length; i++) 
        {
            deathRB[i].isKinematic = false;
            deathRB[i].AddForce(transform.forward * 1000f);
        }
        FadeObj();
        SpawnDeathFX();
    }

    private void FadeObj()
    {
        if (flickerDisappear == null) {
            Debug.Log("FlickerDisappear script not added");
            return;
        }
        flickerDisappear.Disappear();
    }

    private void SpawnDeathFX()
    {
        if (hideObjects.Length != 0) {
            for (int i = 0; i < hideObjects.Length; i++) 
            {
                hideObjects[i].SetActive(false);
            }
        }

        if (DeathSpawn.Length == 0) return;
        for (int i = 0; i < DeathSpawn.Length; i ++) {
            if (DeathSpawn[i] == null) continue;
            var pos = transform.position;
            Instantiate(DeathSpawn[i], new Vector3(pos.x, pos.y + spawnYOffset, pos.z), Quaternion.identity);
        };
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
