using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTarget : MonoBehaviour
{
    [SerializeField] private ParticleSystem spark;
    private static int counter;
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
