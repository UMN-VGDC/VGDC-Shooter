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

    [SerializeField] private GameObject DeathFX;
    [SerializeField] private GameObject[] hideObjects;

    private Animator animator;
    private Rigidbody[] deathRB;
    private int disappearDelay = 2500;

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
    }

    public void KillEnemy()
    {
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

    private async void DeathRagdoll()
    {
        for (int i = 0; i < deathRB.Length; i++) 
        {
            deathRB[i].isKinematic = false;
            deathRB[i].AddForce(transform.forward * 1000f);
        }
        SpawnDeathFX();
        await UniTask.DelayFrame(disappearDelay);
        FlickerDisappear();

    }

    private async void DeathAnimation()
    {
        animator.SetBool("isDeathAnimation", true);
        SpawnDeathFX();
        await UniTask.DelayFrame(disappearDelay);
        FlickerDisappear();
    }

    private async void FlickerDisappear()
    {
        var rend = GetComponentsInChildren<Renderer>();
        for (int r = 0; r < 15; r++)
        {
            for (int i = 0; i < rend.Length; i++)
            {
                rend[i].enabled = false;
            }
            await UniTask.DelayFrame(5);
            for (int i = 0; i < rend.Length; i++)
            {
                rend[i].enabled = true;
            }
            await UniTask.DelayFrame(5);
        }
        Destroy(gameObject);
    }

    private void SpawnDeathFX()
    {
        if (hideObjects.Length != 0) {
            for (int i = 0; i < hideObjects.Length; i++) 
            {
                hideObjects[i].SetActive(false);
            }
        }

        if (DeathFX != null) {
            Instantiate(DeathFX, transform.position, Quaternion.identity);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
