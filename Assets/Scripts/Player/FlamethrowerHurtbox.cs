using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerHurtbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var root = other.transform.root;
        if (root.tag != "Entity" && root.tag != "Boss Enemy") return;
        root.GetComponent<EntityHealth>().DecreaseHealth(1);
    }


}
