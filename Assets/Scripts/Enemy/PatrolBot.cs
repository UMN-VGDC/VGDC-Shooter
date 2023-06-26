using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;

[RequireComponent(typeof(TriggerEventsOnClose))]
public class PatrolBot : MonoBehaviour
{
    [SerializeField] private ParticleSystem targetTimer;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform gunPos, pivot;
    private int attackDelay = 1000;
    private bool isExit;

    public async void QueueAttack()
    {
        isExit = true;
        await Task.Delay(attackDelay);
        if (!isExit || targetTimer == null || targetTimer.isPlaying) return;
        targetTimer.Play();
    }

    public void Attack()
    {
        if (targetTimer == null) return;
        muzzleFlash.Play();
        pivot.DOLocalMoveZ(-2f, 0f);
        pivot.DOLocalMoveZ(0f, 1f);
    }

    public void CancelAttack()
    {
        isExit = false;
    }
}
