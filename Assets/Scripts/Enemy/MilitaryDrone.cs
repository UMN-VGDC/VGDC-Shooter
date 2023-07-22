using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;

public class MilitaryDrone : MultiTargetEnemy
{

    private Animator animator;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
    }

    protected override void DecrementTargetReaction()
    {
        animator.SetTrigger("Stun");
        DOVirtual.Float(0.3f, 0f, 1f, e =>
        {
            animator.SetLayerWeight(1, e);
        });
    }

    protected override void TargetDestroyReaction()
    {
        animator.SetTrigger("Stun");
        DOVirtual.Float(0.3f, 0f, 1f, e =>
        {
            animator.SetLayerWeight(1, e);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void EnemyAttack()
    {
        Debug.Log("Military Drone Attacks!");
    }
}
