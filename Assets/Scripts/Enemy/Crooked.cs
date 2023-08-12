using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Crooked : AttackOnClose
{
    protected override void Start()
    {
        base.Start();
    }

    private bool isDead;
    protected override void EnemyAttack()
    {
        int random = Random.Range(1, 4);
        if (random == 3)
        {
            animator.SetLayerWeight(2, 1);
            animator.SetTrigger("Attack3");
            JumpAnimationOverride();
        } 
        else
        {
            animator.SetTrigger($"Attack{random}");
        }
    }

    private async void JumpAnimationOverride()
    {
        await Task.Delay(3000);
        if (isDead) return;
        DOVirtual.Float(1, 0, 1f, e =>
        {
            animator.SetLayerWeight(2, e);
        });

    }

    public void CrookedDisableAnimator()
    {
        animator.SetLayerWeight(1, 0);
        animator.SetLayerWeight(2, 0);
    }

    private void OnDestroy()
    {
        isDead = true;
    }
}
