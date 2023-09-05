using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

[RequireComponent(typeof(TriggerEventsOnClose))]
[RequireComponent (typeof(AudioSource))]
public abstract class MultiTargetEnemy : MonoBehaviour
{

    [SerializeField] protected int targetCount = 6;
    [SerializeField] private int minTargets = 2;
    [SerializeField] private int maxTargets = 4;
    protected AimTarget[] aimTargets;
    protected int currentTargetCount;
    protected bool isAttacking = true;
    protected bool startAttacking;
    private EntityHealth entityHealth;
    protected AudioSource audioSource;

    public static Action multiTargetsInit;
    public static Action multiTargetsTimeout;
    public static Action<int> multiTargetsPoints;
    public static Action multiTargetsShieldDownSound;

    protected GameObject player;

    protected bool isDestroyed;
    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        aimTargets = GetComponentsInChildren<AimTarget>();
        entityHealth = GetComponent<EntityHealth>();
        
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

    private float timerHurryLength = 2.65f;
    private int points;
    private bool enablePrimaryAttack;
    protected async void AttackLoop()
    {
        if (!startAttacking) return;

        //Start primary attack on second Attack Loop
        if (enablePrimaryAttack) PrimaryAttack();
        enablePrimaryAttack = true;

        //Spawn targets
        int targetCount = UnityEngine.Random.Range(minTargets, maxTargets);
        points = targetCount * 100;
        currentTargetCount = targetCount;
        int randomIndex = UnityEngine.Random.Range(0, aimTargets.Length - 1);
        for (int i = randomIndex; i < randomIndex + targetCount; i++)
        {
            int select = (i + 1) % aimTargets.Length;
            aimTargets[select].EnableTarget();
        }
        multiTargetsInit?.Invoke();

        // Cancel "hurry up" sound when this.targetCount decreases
        int targetCountID = this.targetCount;
        float timerHurry = aimTargets[0].GetParticleLifetime() - timerHurryLength;
        await Task.Delay((int)Math.Round(timerHurry * 1000));
        if (!isAttacking || targetCountID != this.targetCount) return;
        if (isDestroyed) return;
        audioSource.Play();

        await Task.Delay((int)Math.Round(timerHurryLength * 1000));
        if (!isAttacking || targetCountID != this.targetCount) return;
        if (isDestroyed) return;
        multiTargetsTimeout?.Invoke();
    }

    protected virtual void Update()
    {
        // Prevent enemy from attacking if too far from player
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance >= 50)
        {
            enablePrimaryAttack = false;
            startAttacking = false;
        }

        foreach (AimTarget target in aimTargets)
        {
            target.gameObject.transform.LookAt(player.transform);
        }
    }

    protected abstract void PrimaryAttack();
    protected abstract void TargetDestroyReaction();
    protected abstract void DecrementTargetReaction();

    public async void DecrementTargetCount()
    {
        foreach (AimTarget target in aimTargets)
        {
            currentTargetCount--;
            if (currentTargetCount == 0) break;
            entityHealth.MultiHitFlash(2, 0.3f, true);
            TargetDestroyReaction();
            return;
        }
        if (targetCount == 0) return;

        //only decrement when all targets are broken
        audioSource.Stop();
        DecrementTargetReaction();
        entityHealth.MultiHitFlash(6);
        targetCount--;
        multiTargetsPoints?.Invoke(points);
        foreach (AimTarget target in aimTargets)
        {
            target.SetText($"{targetCount}");
        }

        if (targetCount == 0)
        {
            gameObject.tag = "Entity";
            isAttacking = false;
            foreach (AimTarget target in aimTargets)
            {
                target.DisableTarget();
            }
            entityHealth.BossWeakenEffect(2f);
            multiTargetsShieldDownSound?.Invoke();
        }
        else
        {
            entityHealth.BossWeakenEffect(0.5f);
        }

        await Task.Delay(1000);
        if (!isAttacking) return;
        AttackLoop();
        audioSource.Stop();
    }


    protected abstract void EnemyAttack();
    private bool currentAttack;
    public async void Attack()
    {
        if (currentAttack) return;
        currentAttack = true;
        EnemyAttack();
        await Task.Delay(1000);
        if (!isAttacking) return;
        currentAttack = false;
        AttackLoop();
    }

    private void OnDestroy()
    {
        isDestroyed = true;
    }

}
