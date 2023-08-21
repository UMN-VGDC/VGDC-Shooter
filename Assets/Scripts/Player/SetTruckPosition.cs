using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetTruckPosition : MonoBehaviour
{
    public int setPosition;
    [SerializeField] private PathCreator pathCreator;

    // Update is called once per frame
    void Update()
    {
        transform.position = pathCreator.path.GetPointAtDistance(setPosition);
        transform.rotation = pathCreator.path.GetRotationAtDistance(setPosition);
    }
}
