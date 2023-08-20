using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Shoot
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform impactParticles;
    [SerializeField] private ParticleSystem trailParticles;
    [SerializeField] private CrossHair crossHair;
    private float trailEmissionDefault;
    private AudioSource audioSource;

    protected override void ShootBullet()
    {
        crossHair.DecreaseEnemyHealth(1, 2);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        isContinuousShoot = true;
        CrossHair.raycastObject += LaserTrail;
        trailEmissionDefault = trailParticles.emission.rateOverTime.constantMax;
    }

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = shootSound;
        audioSource.Play();
    }

    private void LaserTrail(Transform hitPos)
    {
        var emission = trailParticles.emission;
        emission.rateOverTime = hitPos.gameObject.isStatic ? trailEmissionDefault : 0;
    }

    protected override void Update()
    {
        base.Update();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, lookAt.position);
        impactParticles.position = lookAt.position;
    }

    private void OnDestroy()
    {
        CrossHair.raycastObject -= LaserTrail;
    }

}
