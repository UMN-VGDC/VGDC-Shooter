using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterPieces : MonoBehaviour
{

    private Rigidbody[] deathRB;
    [SerializeField] private float forceStrength = 1000f;
    // Start is called before the first frame update
    void Start()
    {
        deathRB = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < deathRB.Length; i++) 
        {
            deathRB[i].AddForce(Random.insideUnitSphere * forceStrength);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
