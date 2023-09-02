using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DummyTarget : MonoBehaviour
{
    [SerializeField] private ParticleSystem spark;
    public static int counter;
    public static Action dummyTargetsDestroyed;

    private void Start()
    {
        counter++;
    }

    public void PlayTargetSound()
    {
        SoundManager.Instance.DummyTargetSound();
        spark.Play();
        counter--;
        if (counter == 0) dummyTargetsDestroyed?.Invoke();
    }
}
