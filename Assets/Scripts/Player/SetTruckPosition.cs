using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetTruckPosition : MonoBehaviour
{
    [SerializeField] private float setPosition;
    [SerializeField] private PathCreator pathCreator;

    [Header("Exit Path")]
    [SerializeField] private SetTruckPosition enterPos;
    [SerializeField] private PathCreator newPath;

    public float GetPosition()
    {
        return setPosition;
    }

    public void GetLength()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pathCreator.path.GetPointAtDistance(setPosition);
        transform.rotation = pathCreator.path.GetRotationAtDistance(setPosition);
    }
}
