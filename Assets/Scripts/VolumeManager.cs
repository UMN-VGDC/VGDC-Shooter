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
        EnemyDeath.killPoints += ContrastPop;
    }

    private void DamageVignette()
    {
        Vignette vignette;
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            DOVirtual.Float(0.5f, 0f, 1f, e =>
            {
                vignette.intensity.value = e;
            });
        }
    }

    private void ContrastPop(int i)
    {
        ColorAdjustments colorAdjustmnets;
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustmnets))
        {
            DOVirtual.Float(0f, 1f, 0.4f, e =>
            {
                colorAdjustmnets.postExposure.value = e;
            });
            DOVirtual.Float(100f, 14f, 0.4f, e =>
            {
                colorAdjustmnets.contrast.value = e;
            });
        }
        Bloom bloom;
        if (volume.profile.TryGet<Bloom>(out bloom))
        {
            DOVirtual.Float(2f, 1f, 0.4f, e =>
            {
                bloom.intensity.value = e;
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
        EnemyDeath.killPoints -= ContrastPop;
    }
}
