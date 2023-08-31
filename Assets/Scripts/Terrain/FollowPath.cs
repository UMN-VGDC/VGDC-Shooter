using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private float initialSpeed;
    //[SerializeField] private float initialPosition;
    [SerializeField] private SetTruckPosition setPosition;
    private float distanceTravelled, speedTarget, speed;
    private bool isMoving;
    private Vector3 endPoint;

    // Start is called before the first frame update
    void Start()
    {
        speed = initialSpeed;
        speedTarget = initialSpeed;
        distanceTravelled += setPosition.GetPosition();
        DummyTarget.dummyTargetsDestroyed += StartMoving;
        endPoint = pathCreator.path.GetPointAtTime(0.999f);
        Debug.Log(endPoint);
    }

    private async void StartMoving()
    {
        await Task.Delay(1000);
        isMoving = true;
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
        if (!isMoving) return;
        speed = Mathf.MoveTowards(speed, speedTarget, Time.deltaTime * 2);
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);

        float distance = Vector3.Distance(transform.position, endPoint);
        Debug.Log(distance);
    }

    private void OnDestroy()
    {
        DummyTarget.dummyTargetsDestroyed -= StartMoving;
    }
}
