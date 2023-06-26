using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ParticleSystemCallback : MonoBehaviour
{
    [SerializeField] private UnityEvent particleSystemCallback;
    public void OnParticleSystemStopped()
    {
        particleSystemCallback.Invoke();
    }

}
