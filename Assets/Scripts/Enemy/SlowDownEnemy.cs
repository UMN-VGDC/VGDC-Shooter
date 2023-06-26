using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlowDownEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var root = other.gameObject.transform.root;
        if (root.tag != "Entity") return;
        if (root.TryGetComponent<TriggerEventsOnClose>(out TriggerEventsOnClose triggerEventsOnClose))
        {
            triggerEventsOnClose.TriggerEnter();
        }
        //if (root.TryGetComponent<Chase>(out Chase chase))
        //{
        //    chase.SlowSpeed();
        //}

        //if (root.TryGetComponent<FlyingEnemyTransform>(out FlyingEnemyTransform flyingEnemyTransform))
        //{
        //    flyingEnemyTransform.lowerEnemy();
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        var root = other.gameObject.transform.root;
        if (root.tag != "Entity") return;
        if (root.TryGetComponent<TriggerEventsOnClose>(out TriggerEventsOnClose triggerEventsOnClose))
        {
            triggerEventsOnClose.TriggerExit();
        }
        //if (root.TryGetComponent<Chase>(out Chase chase))
        //{
        //    chase.ResetSpeed();
        //}

        //if (root.TryGetComponent<FlyingEnemyTransform>(out FlyingEnemyTransform flyingEnemyTransform))
        //{
        //    flyingEnemyTransform.raiseEnemy();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
