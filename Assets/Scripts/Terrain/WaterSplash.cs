using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class WaterSplash : MonoBehaviour
{
    [SerializeField] private GameObject waterSplashVFX;
    public static Action splashSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Debris") return;
        if (other.tag == "Bullet")
        {
            GameObject smallSplash = Instantiate(waterSplashVFX, other.transform.position, Quaternion.identity) as GameObject;
            smallSplash.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else
        {
            Instantiate(waterSplashVFX, other.transform.position, Quaternion.identity);
            splashSound?.Invoke();
            Destroy(other.gameObject);
        }
    }

}
