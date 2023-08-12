using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class AttackOnClose : MonoBehaviour
{
    protected Animator animator;
    private Transform playerPos;
    [SerializeField] private float attackDistance = 7.5f;
    [SerializeField] private int attackQueue = 4000;
    private bool isPlaying;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected abstract void EnemyAttack();

    private async void AttackQueue()
    {
        if (isPlaying) return;
        isPlaying = true;
        EnemyAttack();
        await Task.Delay(attackQueue);
        isPlaying = false;

    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, playerPos.position);
        if (distance < attackDistance)
        {
            AttackQueue();
        }
    }
}
