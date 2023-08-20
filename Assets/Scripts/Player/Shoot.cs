using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public abstract class Shoot : MonoBehaviour
{

    [SerializeField] private int fireRate = 30;
    [SerializeField] protected AudioClip shootSound;
    [SerializeField] private Transform leftHandTransform, rightHandTransform;
    private float _currentFireCountdown = 1f;
    private Transform leftIK, rightIK;
    protected Transform lookAt;
    protected bool isContinuousShoot;
    protected bool isLookAt = true;

    public static Action shootBullet;
    public static Action<AudioClip> setShootSound;

    protected virtual void Start()
    {
        lookAt = GameObject.FindGameObjectWithTag("Gun Aim Target").transform;
        leftIK = GameObject.FindGameObjectWithTag("Left Hand IK").transform;
        rightIK = GameObject.FindGameObjectWithTag("Right Hand IK").transform;
        setShootSound?.Invoke(shootSound);
    }

    private void OnEnable()
    {
        setShootSound?.Invoke(shootSound);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isLookAt) transform.LookAt(lookAt);
        _currentFireCountdown = Mathf.MoveTowards(_currentFireCountdown, 0f, Time.deltaTime * fireRate);
        if (_currentFireCountdown <= 0f)
        {
            ShootBullet();
            if (!isContinuousShoot) shootBullet?.Invoke();
            _currentFireCountdown = 1f;
        }

        leftIK.position = leftHandTransform.position;
        leftIK.rotation = leftHandTransform.rotation;
        rightIK.position = rightHandTransform.position;
        rightIK.rotation = rightHandTransform.rotation;
    }

    protected abstract void ShootBullet();
}
