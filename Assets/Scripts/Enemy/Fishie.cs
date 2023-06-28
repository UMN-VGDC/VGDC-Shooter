using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Threading.Tasks;
using System;
using DG.Tweening;
using Cysharp.Threading.Tasks.CompilerServices;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(TriggerEventsOnClose))]
public class Fishie : MonoBehaviour
{
    private MultiAimConstraint[] multiAimConstraint;
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletSpawner;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip timeoutSound;


    private bool isAttacking = true;
    private bool startAttacking;
    private GameObject player;
    private Animator animator;

    [SerializeField] private int targetCount = 6;
    private AimTarget[] aimTargets;
    private int currentTargetCount;

    public static Action fishieTargets;
    public static Action<AudioClip> fishieShoot;
    public static Action<AudioClip> fishieTargetTimeoutSound;
    public static Action fishieTargetTimerHurrySound;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        multiAimConstraint = rigBuilder.gameObject.GetComponentsInChildren<MultiAimConstraint>();
        foreach (MultiAimConstraint m in multiAimConstraint)
        {
            var data = m.data.sourceObjects;
            data.Add(new WeightedTransform(player.transform, 0.8f));
            m.data.sourceObjects = data;
        }
        rigBuilder.Build();

        aimTargets = GetComponentsInChildren<AimTarget>();
        foreach (AimTarget target in aimTargets)
        {
            target.DisableTarget();
        }
    }

    public void StartAttacking()
    {
        if (startAttacking) return;
        startAttacking = true;
        foreach (AimTarget target in aimTargets)
        {
            target.SetText($"{targetCount}");
        }
        AttackLoop();
    }

    private async void AttackLoop()
    {
        int targetCount = UnityEngine.Random.Range(2, 4);
        currentTargetCount = targetCount;
        int randomIndex = UnityEngine.Random.Range(0, aimTargets.Length - 1);
        for (int i = randomIndex; i < randomIndex + targetCount; i++)
        {
            int select = (i + 1) % aimTargets.Length;
            aimTargets[select].EnableTarget();
        }
        fishieTargets?.Invoke();

        // Cancel "hurry up" sound when this.targetCount decreases
        int targetCountID = this.targetCount;
        float timerHurry = aimTargets[0].GetParticleLifetime() - 2.65f;
        await Task.Delay((int)Math.Round(timerHurry * 1000));
        if (!isAttacking || targetCountID != this.targetCount) return;
        audioSource.Play();
    }

    private bool currentAttack;
    public async void Attack()
    {
        if (currentAttack) return;
        currentAttack = true;
        animator.SetTrigger("Attack");
        fishieShoot?.Invoke(shootSound);
        fishieTargetTimeoutSound?.Invoke(timeoutSound);
        await Task.Delay(200);

        if (!isAttacking) return;
        Instantiate(bullet, bulletSpawner.transform.position, Quaternion.identity);
        bulletSpawner.GetComponent<ParticleSystem>().Play();
        await Task.Delay(1000);
        if (!isAttacking) return;
        currentAttack = false;
        AttackLoop();
    }


    public async void DecrementTargetCount()
    {
        foreach (AimTarget target in aimTargets)
        {
            currentTargetCount--;
            if (currentTargetCount == 0) break;
            GetComponent<EntityHealth>().MultiHitFlash(2);
            animator.SetTrigger("Stun");
            DOVirtual.Float(0.3f, 0f, 1f, e =>
            {
                animator.SetLayerWeight(2, e);
            });
            return;
        }
        if (targetCount == 0) return;

        //only decrement when all targets are broken
        audioSource.Stop();
        GetComponent<EntityHealth>().MultiHitFlash(6);
        targetCount--;
        foreach (AimTarget target in aimTargets)
        {
            target.SetText($"{targetCount}");
        }

        animator.SetTrigger("Stun");
        DOVirtual.Float(1f, 0f, 1f, e =>
        {
            animator.SetLayerWeight(2, e);
        });

        if (targetCount == 0)
        {
            gameObject.tag = "Entity";
            isAttacking = false;
            foreach (AimTarget target in aimTargets)
            {
                target.DisableTarget();
            }
        }

        await Task.Delay(1000);
        if (!isAttacking) return;
        AttackLoop();
        audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(AimTarget target in aimTargets)
        {
           target.gameObject.transform.LookAt(player.transform);
        }
    }
}
