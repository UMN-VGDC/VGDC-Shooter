using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private Volume volume;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.damageTaken += DamageVignette;
    }

    private void DamageVignette()
    {
        Vignette vignette;
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            DOVirtual.Float(0.3f, 0f, 1f, e =>
            {
                vignette.intensity.value = e;
            });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        PlayerHealth.damageTaken -= DamageVignette;
    }
}
