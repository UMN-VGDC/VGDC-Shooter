using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using System;

[RequireComponent(typeof(TriggerEventsOnClose))]
public class PatrolBot : MonoBehaviour
{
    [SerializeField] private ParticleSystem targetTimer;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform gunPos, pivot;
    [SerializeField] private AudioClip patrolShootSound;
    [SerializeField] private GameObject bullet;
    private int attackDelay = 1000;
    private bool isExit;

    private AudioSource audioSource;

    public static Action<AudioClip> patrolShoot;
    public static Action patrolCharge;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public async void QueueAttack()
    {
        isExit = true;
        await Task.Delay(attackDelay);
        if (!isExit || targetTimer == null || targetTimer.isPlaying) return;
        targetTimer.Play();
        patrolCharge?.Invoke();
        audioSource.Play();
    }

    public void Attack()
    {
        if (targetTimer == null) return;
        audioSource.Stop();
        muzzleFlash.Play();
        patrolShoot?.Invoke(patrolShootSound);
        Instantiate(bullet, gunPos.position, Quaternion.identity);
        pivot.DOLocalMoveZ(-2f, 0f);
        pivot.DOLocalMoveZ(0f, 1f);
    }

    public void CancelAttack()
    {
        isExit = false;
    }
}
