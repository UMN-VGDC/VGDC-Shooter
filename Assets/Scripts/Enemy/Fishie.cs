using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Threading.Tasks;
using System;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class Fishie : MultiTargetEnemy
{
    private MultiAimConstraint[] multiAimConstraint;
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletSpawner;
    [SerializeField] private AudioClip shootSound;

    private Animator animator;
    public static Action<AudioClip> fishieShoot;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        multiAimConstraint = rigBuilder.gameObject.GetComponentsInChildren<MultiAimConstraint>();
        foreach (MultiAimConstraint m in multiAimConstraint)
        {
            var data = m.data.sourceObjects;
            data.Add(new WeightedTransform(player.transform, 0.8f));
            m.data.sourceObjects = data;
        }
        rigBuilder.Build();
    }


    protected override void TargetDestroyReaction()
    {
        animator.SetTrigger("Stun");
        DOVirtual.Float(0.3f, 0f, 1f, e =>
        {
            animator.SetLayerWeight(2, e);
        });
    }

    protected override void DecrementTargetReaction()
    {
        animator.SetTrigger("Stun");
        DOVirtual.Float(1f, 0f, 1f, e =>
        {
            animator.SetLayerWeight(2, e);
        });
    }

    protected override async void EnemyAttack()
    {
        animator.SetTrigger("Attack");
        fishieShoot?.Invoke(shootSound);
        await Task.Delay(200);
        if (!isAttacking) return;
        Instantiate(bullet, bulletSpawner.transform.position, Quaternion.identity);
        bulletSpawner.GetComponent<ParticleSystem>().Play();
    }
}
