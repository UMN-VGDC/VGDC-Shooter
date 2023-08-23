using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private float initialSpeed;
    //[SerializeField] private float initialPosition;
    [SerializeField] private SetTruckPosition setPosition;
    private float distanceTravelled, speedTarget, speed;
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        speed = initialSpeed;
        speedTarget = initialSpeed;
        distanceTravelled += setPosition.setPosition;
        GameManager.shootingStart += StartMoving;
    }

    private void StartMoving() => isMoving = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "SpeedTrigger") return;
        speedTarget = other.gameObject.GetComponent<SpeedMultiplier>().speedTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "SpeedTrigger") return;
        speedTarget = initialSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving) return;
        speed = Mathf.MoveTowards(speed, speedTarget, Time.deltaTime * 2);
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }

    private void OnDestroy()
    {
        GameManager.shootingStart -= StartMoving;
    }
}
