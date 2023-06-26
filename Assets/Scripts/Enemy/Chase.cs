using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(TriggerEventsOnClose))]
public class Chase : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private GameObject player;
    [SerializeField] private float slowSpeed = 12f;
    [HideInInspector] public float initialSpeed;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        initialSpeed = navMeshAgent.speed;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void ResetSpeed()
    {
        navMeshAgent.speed = initialSpeed;
    }

    public void SlowSpeed()
    {
        navMeshAgent.speed = slowSpeed;
    }
    public void SetSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.destination = player.transform.position;
    }
}
