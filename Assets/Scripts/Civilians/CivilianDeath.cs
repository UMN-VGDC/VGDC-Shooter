using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CivilianDeath : MonoBehaviour
{
    private Rigidbody[] deathRB;
    
    void Start()
    {
        deathRB = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < deathRB.Length; i++) 
        {
            deathRB[i].isKinematic = true;
        }
        DeathTimer();
    }

    private async void DeathTimer()
    {
        await UniTask.DelayFrame(2000);
        for (int i = 0; i < deathRB.Length; i++) 
        {
            deathRB[i].isKinematic = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
