using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualPistols : Shoot
{
    private bool alternate;
    [SerializeField] private Transform[] gunTransforms;
    [SerializeField] private Transform[] gunTransformsAnimate;
    [SerializeField] private Transform[] bulletSpawners;
    [SerializeField] private GameObject bullet;
    [SerializeField] private ParticleSystem[] muzzleFlash;

    protected override void Start()
    {
        base.Start();
        isLookAt = false;
    }
    protected override void ShootBullet()
    {
        alternate = !alternate;
        int index = alternate ? 0 : 1;
        Instantiate(bullet, bulletSpawners[index].position, bulletSpawners[index].rotation);
        muzzleFlash[index].Play();

        DOTween.Kill(gunTransformsAnimate[index]);
        Sequence s = DOTween.Sequence();
        s.Join(gunTransformsAnimate[index].DOLocalMoveY(0.1f, 0f));
        s.Join(gunTransformsAnimate[index].DOLocalRotate(new Vector3(25, 180, 0), 0f));
        s.Append(gunTransformsAnimate[index].DOLocalMoveY(0, 0.1f));
        s.Join(gunTransformsAnimate[index].DOLocalRotate(new Vector3(0, 180, 0), 0.2f));
    }

    protected override void Update()
    {
        base.Update();
        if (!isShooting) return;
        for (int i = 0; i < gunTransforms.Length; i++)
        {
            gunTransforms[i].LookAt(lookAt);
        }
    }
}
