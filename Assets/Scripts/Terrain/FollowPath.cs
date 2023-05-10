using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private float initialSpeed;
    private float distanceTravelled, speedTarget, speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = initialSpeed;
    }

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
        speed = Mathf.MoveTowards(speed, speedTarget, Time.deltaTime * 2);
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }
}
