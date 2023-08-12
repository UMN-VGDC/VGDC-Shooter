using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

[RequireComponent(typeof(EntityHealth))]
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
    [SerializeField] private Transform deathSpawnPos;
    [SerializeField] private float spawnYOffset;
    [SerializeField] private GameObject[] hideObjects;
    [SerializeField] private AudioClip[] deathSounds;

    public static Action<AudioClip[]> deathSound;

    private Animator animator;
    private Rigidbody[] deathRB;
    private FlickerDisappear flickerDisappear;

    private Transform playerPos;

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
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, playerPos.position);
        if (distance > 90f)
        {
            GetComponent<EntityHealth>().AddLoad();
            Destroy(gameObject);
        }
    }

    public void KillEnemy()
    {
        gameObject.tag = "Untagged";
        deathSound?.Invoke(deathSounds);
        if (TryGetComponent<Chase>(out Chase chase))
        {
            chase.SetSpeed(0);
        }

        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource.Stop();
        }

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
        animator.enabled = false;
        for (int i = 0; i < deathRB.Length; i++) 
        {
            deathRB[i].isKinematic = false;
            deathRB[i].AddForce(transform.forward * -1000f);
            deathRB[i].gameObject.tag = "Untagged";
        }
        SpawnDeathFX();
        FadeObj();
    }

    private void DeathAnimation()
    {
        animator.SetBool("isDeathAnimation", true);
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders )
        {
            c.enabled = false;
        }
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
        Vector3 pos = deathSpawnPos ? deathSpawnPos.position : transform.position;
        for (int i = 0; i < DeathSpawn.Length; i ++) {
            if (DeathSpawn[i] == null) continue;
            Quaternion randomRot = Quaternion.Euler(0, UnityEngine.Random.Range(-180, 180), 0);
            Instantiate(DeathSpawn[i], new Vector3(pos.x, pos.y + spawnYOffset, pos.z), randomRot);
        };
    }
}
