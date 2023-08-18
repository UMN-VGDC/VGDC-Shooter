using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyUIAnimation : MonoBehaviour
{

    private Transform moneyTarget;
    [SerializeField] private Transform lookAt;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float rotationSpeed = 80f;
    [SerializeField] private GameObject particles;
    [SerializeField] private AudioClip moneySound;
    [SerializeField] private int moneyValue = 1;
    private float rotationSpeedup, slowSpeed, distance;
    public static Action<AudioClip> playMoneySound;
    public static Action<int> moneyAdd;

    // Start is called before the first frame update
    void Start()
    {
        moneyTarget = GameObject.FindGameObjectWithTag("Money Target").transform;
        rotationSpeedup = rotationSpeed * 4f;
        slowSpeed = speed * 0.6f;
        distance = Vector3.Distance(transform.position, moneyTarget.position);
    }

    // Update is called once per frame
    void Update()
    {
        float currentDistance = Vector3.Distance(transform.position, moneyTarget.position);
        float currentRotationSpeed = Mathf.Lerp(rotationSpeedup, rotationSpeed, currentDistance / distance);
        float currentSpeed = Mathf.Lerp(slowSpeed, speed, currentDistance / distance);
        lookAt.LookAt(moneyTarget.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAt.rotation, currentRotationSpeed * Time.deltaTime);
        transform.position += transform.forward * Time.deltaTime * currentSpeed;
        if (currentDistance < 0.05f ) 
        {
            GameObject instantiatedParticles = Instantiate(particles, transform.position, Quaternion.identity);
            instantiatedParticles.transform.parent = moneyTarget;
            playMoneySound?.Invoke(moneySound);
            moneyAdd?.Invoke(moneyValue);
            Destroy(gameObject);
        }
    }
}
