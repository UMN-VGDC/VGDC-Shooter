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
    private float endDistance;
    private PathCreator mainPath;

    // Start is called before the first frame update
    void Start()
    {
        speed = initialSpeed;
        speedTarget = initialSpeed;
        distanceTravelled += setPosition.GetPosition();
        DummyTarget.dummyTargetsDestroyed += StartMoving;
        GameObject endPoint = pathCreator.transform.Find("Exit Position").gameObject;
        endDistance = pathCreator.path.GetClosestDistanceAlongPath(endPoint.transform.position);
        mainPath = GameObject.FindGameObjectWithTag("Main Path").GetComponent<PathCreator>();
    }

    private async void StartMoving()
    {
        await Task.Delay(1000);
        isMoving = true;
        await Task.Delay(3000);
        GameManager.Instance.UpdateGameState(GameState.Shooting);
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

        if (distanceTravelled >= endDistance && pathCreator != mainPath)
        {
            distanceTravelled = mainPath.path.GetClosestDistanceAlongPath(transform.position);
            pathCreator = mainPath;
        }
    }

    private void OnDestroy()
    {
        DummyTarget.dummyTargetsDestroyed -= StartMoving;
    }
}
