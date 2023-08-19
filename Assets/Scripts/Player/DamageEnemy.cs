using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    public static Action crit;
    public static Action<int> killPoints;

    protected void EntityHit(GameObject entity, int amount = 1, int critAmount = 2)
    {
        var root = entity.transform.root;
        if (entity.tag == "Head" && root.tag == "Entity")
        {
            killPoints?.Invoke(root.GetComponent<EntityHealth>().DecreaseHealth(critAmount));
            crit?.Invoke();
            return;
        }

        if (root.tag == "Entity")
        {
            killPoints?.Invoke(root.GetComponent<EntityHealth>().DecreaseHealth(amount));
            return;
        }

        if (entity.tag == "Entity")
        {
            killPoints?.Invoke(entity.GetComponent<EntityHealth>().DecreaseHealth(amount));
        }

    }
}
