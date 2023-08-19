using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBlaster : Shoot
{
    [SerializeField] private Transform spawner;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform recoilObject;
    [SerializeField] private float recoilStrength = 5f;
    [SerializeField] private ParticleSystem muzzleFlash;

    protected override void Start()
    {
        base.Start();
    }

    protected override void ShootBullet()
    {
        Instantiate(bullet, spawner.position, transform.rotation);
        muzzleFlash.Play();
        recoilObject.localPosition = new Vector3(0, 0, 0);
        DOTween.Kill(recoilObject);
        recoilObject.DOLocalMoveZ(0f - (0.1f * recoilStrength), 0.01f);
    }

}
