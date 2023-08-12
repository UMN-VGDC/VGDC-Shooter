using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crockie : AttackOnClose
{
    [SerializeField] private AudioClip roar;
    public static Action<AudioClip> crockieRoar;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void EnemyAttack()
    {
        animator.SetTrigger("Attack");
        crockieRoar?.Invoke(roar);
    }
}
