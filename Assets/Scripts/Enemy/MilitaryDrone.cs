using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Animations.Rigging;
using System;

public class MilitaryDrone : MultiTargetEnemy
{

    private Animator animator;
    [SerializeField] private Transform[] missileSpawners;
    [SerializeField] private GameObject missile;
    [SerializeField] private int missilCount = 3;
    [SerializeField] private Transform bulletSpawner;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int shootCount = 3;
    [SerializeField] private Transform gunIkTarget;
    [SerializeField] private RigBuilder rigBuilder;
    private MultiAimConstraint multiAimConstraint;
    [SerializeField] private Transform gunLookAt;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private ParticleSystem muzzleFlashVFX;

    private Vector3 gunIkTargetDefault;
    private int currentShootCount;
    public static Action<AudioClip> militaryDroneShootSound;

    public static int militaryDroneCount;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        militaryDroneCount++;
        currentShootCount = shootCount;
        Vector3 d = gunIkTarget.transform.localPosition;
        gunIkTargetDefault = new Vector3(d.x, d.y, d.z + 0.8f);
        gunIkTarget.transform.localPosition = gunIkTargetDefault;

        multiAimConstraint = GetComponentInChildren<MultiAimConstraint>();
        var data = multiAimConstraint.data.sourceObjects;
        data.Add(new WeightedTransform(player.transform, 0.8f));
        multiAimConstraint.data.sourceObjects = data;
        rigBuilder.Build();
    }

    private void Update()
    {
        gunIkTarget.rotation = Quaternion.Lerp(gunIkTarget.rotation, gunLookAt.rotation, Time.deltaTime * 5);
    }

    protected override void DecrementTargetReaction()
    {
        StunAnimation();
    }
    protected override void TargetDestroyReaction()
    {
        StunAnimation();
    }

    private void StunAnimation()
    {
        animator.SetTrigger("Stun");
        DOVirtual.Float(0.3f, 0f, 1f, e =>
        {
            animator.SetLayerWeight(1, e);
        });
    }

    protected override async void EnemyAttack()
    {

        Instantiate(bullet, bulletSpawner.position, bulletSpawner.rotation);
        currentShootCount--;
        muzzleFlashVFX.Play();
        militaryDroneShootSound?.Invoke(shootSound);

        //gun recoil
        DOTween.Kill(gunIkTarget.transform);
        Vector3 g = gunIkTargetDefault;
        gunIkTarget.localPosition = new Vector3(g.x, g.y, g.z - 1.2f);
        gunIkTarget.DOLocalMoveZ(gunIkTargetDefault.z, 0.5f);

        if (currentShootCount == 0)
        {
            currentShootCount = shootCount;
            return;
        }
        await Task.Delay(380);
        EnemyAttack();
    }

    protected override void PrimaryAttack()
    {
        int selectRandom = UnityEngine.Random.Range(0, missileSpawners.Length - 1);
        FireMissiles(selectRandom);
    }

    private int currentIndex;
    private async void FireMissiles(int randomIndex)
    {
        if (isDestroyed) return;
        randomIndex += currentIndex;
        int index = randomIndex % missileSpawners.Length;
        Instantiate(missile, missileSpawners[index].position, missileSpawners[index].rotation);
        missileSpawners[index].GetComponentInChildren<ParticleSystem>().Play();
        if (currentIndex == missilCount - 1 || Missile.missileCount > militaryDroneCount + 1)
        {
            currentIndex = 0;
            return;
        }

        await Task.Delay(200);
        currentIndex++;
        FireMissiles(randomIndex);

    }

    private void OnDestroy()
    {
        militaryDroneCount--;
    }

}
