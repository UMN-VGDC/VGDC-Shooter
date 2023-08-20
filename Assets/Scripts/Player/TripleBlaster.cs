using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleBlaster : Shoot
{
    [SerializeField] private ParticleSystem[] muzzleFlash;
    [SerializeField] private Transform[] spawner;
    [SerializeField] private GameObject bullet;

    protected override void ShootBullet()
    {
        for (int i = 0; i < muzzleFlash.Length; i++)
        {
            muzzleFlash[i].Play();
        }

        for (int i = 0; i < spawner.Length; i++)
        {
            Instantiate(bullet, spawner[i].position, spawner[i].rotation);
        }
    }
}
