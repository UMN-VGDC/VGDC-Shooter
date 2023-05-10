using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlowDownEnemy : MonoBehaviour
{
    [SerializeField] private float slowSpeed = 12f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var root = other.gameObject.transform.root;
        if (root.tag != "Entity") return;
        if (root.TryGetComponent<Chase>(out Chase chase))
        {
            chase.SetSpeed(slowSpeed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var root = other.gameObject.transform.root;
        if (root.tag != "Entity") return;
        if (root.TryGetComponent<Chase>(out Chase chase))
        {
            chase.ResetSpeed();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
