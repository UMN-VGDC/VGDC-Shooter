using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlowDownEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var root = other.gameObject.transform.root;
        if (root.tag != "Entity" && root.tag != "Boss Enemy") return;
        if (root.TryGetComponent<TriggerEventsOnClose>(out TriggerEventsOnClose triggerEventsOnClose))
        {
            triggerEventsOnClose.TriggerEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var root = other.gameObject.transform.root;
        if (root.tag != "Entity" && root.tag != "Boss Enemy") return;
        if (root.TryGetComponent<TriggerEventsOnClose>(out TriggerEventsOnClose triggerEventsOnClose))
        {
            triggerEventsOnClose.TriggerExit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
