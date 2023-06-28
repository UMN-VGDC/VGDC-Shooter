using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Threading.Tasks;
using System;
using DG.Tweening;

[RequireComponent(typeof(TriggerEventsOnClose))]
public class Fishie : MonoBehaviour
{

    private MultiAimConstraint[] multiAimConstraint;
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private GameObject[] targets;
    [SerializeField] private int targetCount = 6;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletSpawner;
    private Animator animator;
    private bool isAttacking = true;
    private bool startAttacking;
    private GameObject player;
    private int currentTargetCount;

    public static Action fishieTargets;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponentInChildren<Animator>();
        multiAimConstraint = rigBuilder.gameObject.GetComponentsInChildren<MultiAimConstraint>();
        foreach (MultiAimConstraint m in multiAimConstraint)
        {
            var data = m.data.sourceObjects;
            data.Add(new WeightedTransform(player.transform, 0.8f));
            m.data.sourceObjects = data;
        }

        foreach (GameObject target in targets)
        {
            target.GetComponent<AimTarget>().DisableTarget();
        }
        rigBuilder.Build();
    }

    public void StartAttacking()
    {
        if (startAttacking) return;
        startAttacking = true;
        foreach (GameObject t in targets)
        {
            t.GetComponent<AimTarget>().SetText($"{targetCount}");
        }
        AttackLoop();
    }

    private void AttackLoop()
    {
        int targetCount = UnityEngine.Random.Range(2, 4);
        currentTargetCount = targetCount;
        int randomIndex = UnityEngine.Random.Range(0, targets.Length - 1);
        for (int i = randomIndex; i < randomIndex + targetCount; i++)
        {
            int select = (i + 1) % targets.Length;
            targets[select].GetComponent<AimTarget>().EnableTarget();
        }
        fishieTargets?.Invoke();
    }

    private bool currentAttack;
    public async void Attack()
    {
        if (currentAttack) return;
        currentAttack = true;
        animator.SetTrigger("Attack");
        await Task.Delay(200);

        if (!isAttacking) return;
        Instantiate(bullet, bulletSpawner.transform.position, Quaternion.identity);
        bulletSpawner.GetComponent<ParticleSystem>().Play();
        await Task.Delay(1000);
        if (!isAttacking) return;
        currentAttack = false;
        AttackLoop();
    }


    public async void DecrementTargetCount()
    {
        foreach (GameObject t in targets)
        {
            currentTargetCount--;
            if (currentTargetCount == 0) break;
            GetComponent<EntityHealth>().MultiHitFlash(2);
            animator.SetTrigger("Stun");
            DOVirtual.Float(0.3f, 0f, 1f, e =>
            {
                animator.SetLayerWeight(2, e);
            });
            return;
        }
        if (targetCount == 0) return;

        //only decrement when all targets are broken
        GetComponent<EntityHealth>().MultiHitFlash(6);
        targetCount--;
        foreach (GameObject t in targets)
        {
            t.GetComponent<AimTarget>().SetText($"{targetCount}");
        }

        animator.SetTrigger("Stun");
        DOVirtual.Float(1f, 0f, 1f, e =>
        {
            animator.SetLayerWeight(2, e);
        });

        if (targetCount == 0)
        {
            gameObject.tag = "Entity";
            isAttacking = false;
            foreach (GameObject t in targets)
            {
                t.SetActive(false);
            }
        }

        await Task.Delay(1000);
        if (!isAttacking) return;
        AttackLoop();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject t in targets)
        {
           t.transform.LookAt(player.transform);
        }
    }
}
