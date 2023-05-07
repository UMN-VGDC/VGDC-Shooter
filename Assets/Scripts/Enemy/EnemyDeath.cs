using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyDeath : MonoBehaviour
{
    public enum DeathType
    {
        Disappear,
        Ragdoll,
        Animation
    }
    public DeathType deathType;

    [SerializeField] private GameObject[] DeathSpawn;
    [SerializeField] private float spawnYOffset;
    [SerializeField] private GameObject[] hideObjects;

    private Animator animator;
    private Rigidbody[] deathRB;
    private FlickerDisappear flickerDisappear;

    // Start is called before the first frame update
    void Start()
    {
        if (deathType == DeathType.Ragdoll)
        {
            deathRB = GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < deathRB.Length; i++) 
            {
                deathRB[i].isKinematic = true;
            }
        }
        animator = GetComponentInChildren<Animator>();
        flickerDisappear = GetComponent<FlickerDisappear>();
    }

    public void KillEnemy()
    {
        gameObject.tag = "Untagged";
        switch (deathType)
        {
            case DeathType.Disappear:
                DeathDisappear();
                break;
            case DeathType.Ragdoll:
                DeathRagdoll();
                break;
            case DeathType.Animation:
                DeathAnimation();
                break;
        }
    }

    private void DeathDisappear()
    {
        Destroy(gameObject);
        SpawnDeathFX();
    }

    private void DeathRagdoll()
    {
        for (int i = 0; i < deathRB.Length; i++) 
        {
            deathRB[i].isKinematic = false;
            deathRB[i].AddForce(transform.forward * 1000f);
            deathRB[i].gameObject.tag = "Untagged";
        }
        SpawnDeathFX();
        FadeObj();
    }

    private void DeathAnimation()
    {
        animator.SetBool("isDeathAnimation", true);
        SpawnDeathFX();
        FadeObj();
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
