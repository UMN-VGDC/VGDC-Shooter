using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float turnspeed = 100f;
    [SerializeField] private Transform lookAtTransform;
    [SerializeField] private AudioClip missilLaunchSound;

    public static Action<AudioClip> missileLaunchSound;
    public static int missileCount;

    private Transform playerPos;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] getTargets = GameObject.FindGameObjectsWithTag("Player Target");
        playerPos = getTargets[UnityEngine.Random.Range(0, getTargets.Length)].transform;
        missileLaunchSound?.Invoke(missilLaunchSound);
        missileCount++;
    }

    private void Update()
    {
        lookAtTransform.LookAt(playerPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtTransform.rotation, Time.deltaTime * turnspeed);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnDestroy()
    {
        missileCount--;
    }

}
