using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using DG.Tweening;

public class Flamethrowers : MonoBehaviour
{
    private ParticleSystem[] flames;
    private Light[] lights;
    [SerializeField] private Transform hurtbox;
    private Vector3 defaultPos;
    private bool isFiring;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.flameThrower += ActivateFlamethrowers;
        flames = GetComponentsInChildren<ParticleSystem>();
        lights = GetComponentsInChildren<Light>();
        ParticleEmitRate(0);
        SetLightIntensity(0);
        defaultPos = hurtbox.localPosition;
    }

    private async void ActivateFlamethrowers()
    {
        ParticleEmitRate(25f);
        SetLightIntensity(18);
        isFiring = true;
        hurtbox.gameObject.SetActive(true);
        StartFiring();
        await Task.Delay(3000);
        ParticleEmitRate(0);
        SetLightIntensity(0);
        isFiring = false;
        hurtbox.gameObject.SetActive(false);
    }

    private void ParticleEmitRate(float rate)
    {
        foreach (ParticleSystem f in flames)
        {
            var emission = f.emission;
            float current = emission.rateOverTime.constantMin;
            DOVirtual.Float(current, rate, 1, e =>
            {
                emission.rateOverTime = e;
            });
        }
    }

    private void SetLightIntensity(float intensity)
    {
        float currentIntensity = lights[0].intensity;
        DOVirtual.Float(currentIntensity, intensity, 1, e => {
            foreach (Light l in lights) l.intensity = e;
        });
    }

    private void StartFiring()
    {
        DOTween.Kill(hurtbox.position);
        hurtbox.localPosition = defaultPos;
        if (!isFiring) return;
        hurtbox.DOLocalMoveZ(defaultPos.z - 5, 0.1f).OnComplete(() => StartFiring());
    }

    private void OnDestroy()
    {
        SoundManager.flameThrower -= ActivateFlamethrowers;
    }

}
