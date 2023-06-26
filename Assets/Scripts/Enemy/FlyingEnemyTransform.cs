using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerEventsOnClose))]

public class FlyingEnemyTransform : MonoBehaviour
{

    private GameObject player;
    private float Yoffset;
    private Vector3 initialPos;
    [SerializeField] private Transform target;
    [SerializeField] private float randomizeYOffset = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Yoffset = Random.Range(0, randomizeYOffset);
        initialPos = target.localPosition;
        player = GameObject.FindGameObjectWithTag("Player");
        target.localPosition = new Vector3(initialPos.x, initialPos.y + Yoffset, initialPos.z);
    }

    public void lowerEnemy()
    {
        Yoffset = 0f;
    }

    public void raiseEnemy()
    {
        Yoffset = Random.Range(0, randomizeYOffset);
    }

    // Update is called once per frame
    void Update()
    {
        target.LookAt(player.transform.position);
        target.localPosition = Vector3.MoveTowards(target.localPosition, new Vector3(initialPos.x, initialPos.y + Yoffset, initialPos.z), Time.deltaTime * 2);
    }
}
