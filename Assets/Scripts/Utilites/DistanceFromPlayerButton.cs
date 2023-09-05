using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceFromPlayerButton : MonoBehaviour
{
    public void GetPlayerDistance()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log(Vector3.Distance(transform.position, playerTransform.position));
    }
}
