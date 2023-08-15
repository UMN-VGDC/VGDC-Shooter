using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerHurtbox : MonoBehaviour
{
    public static Action<int> killPoints;
    private void OnTriggerEnter(Collider other)
    {
        var root = other.transform.root;
        if (root.tag != "Entity" && root.tag != "Boss Enemy") return;
        killPoints?.Invoke(root.GetComponent<EntityHealth>().DecreaseHealth(1));
    }


}
