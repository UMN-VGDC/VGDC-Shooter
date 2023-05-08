using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private GameObject player;
    public float initialSpeed;

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

    public void SetSpeed(float amount)
    {
        navMeshAgent.speed = amount;
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.destination = player.transform.position;
    }
}
