using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Threading.Tasks;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private bool isFlashbang, isEffectsCancel;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.damageTaken += DamageVignette;
        EntityHealth.enemyDeath += ContrastPop;
        Flashbang.flashbangExplode += FlashBangEffect;
        RandomizeGun.gunSelectGraphic += GunSelectGraphic;
        GameManager.hasDied += DeathSequence;
    }

    private void DeathSequence()
    {
        isEffectsCancel = true;
        Vignette vignette;
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            vignette.intensity.value = 0.8f;
        }

        ColorAdjustments colorAdjustmnets;
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustmnets))
        {
            colorAdjustmnets.saturation.value = -100f;
            DOVirtual.Float(5.5f, 0.2f, 4.5f, e =>
            {
                colorAdjustmnets.postExposure.value = e;
            }).SetUpdate(true);
        }
    }

    private async void GunSelectGraphic()
    {
        Bloom bloom;
        if (volume.profile.TryGet<Bloom>(out bloom))
        {
            Sequence s = DOTween.Sequence();
            s.Append(DOVirtual.Float(0.8f, 0f, 0.6f, e =>
            {
                bloom.intensity.min = e;
            })).SetUpdate(true);

            s.Append(DOVirtual.Float(0f, 0.8f, 0.6f, e =>
            {
                bloom.intensity.min = e;
            }).SetDelay(1f)).SetUpdate(true);
        }

        isEffectsCancel = true;
        await Task.Delay(2000);
        isEffectsCancel = false;
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
        if (i == 0 || isEffectsCancel) return;
        Bloom bloom;
        if (volume.profile.TryGet<Bloom>(out bloom))
        {
            DOVirtual.Float(1.5f, 0.8f, 0.4f, e =>
            {
                bloom.intensity.value = e;
            });
        }
        ColorAdjustments colorAdjustmnets;
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustmnets))
        {
            DOVirtual.Float(100f, 14f, 0.4f, e =>
            {
                colorAdjustmnets.contrast.value = e;
            });

            if (isFlashbang) return;
            DOVirtual.Float(-0.5f, 0.2f, 0.4f, e =>
            {
                colorAdjustmnets.postExposure.value = e;
            });
            
        }

    }

    private async void FlashBangEffect()
    {
        if (isEffectsCancel) return;
        float duration = 4f;
        isFlashbang = true;

        ColorAdjustments colorAdjustmnets;
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustmnets))
        {
            DOVirtual.Float(5.2f, 0.2f, duration, e =>
            {
                colorAdjustmnets.postExposure.value = e;
            });
        }
        MotionBlur motionBlur;
        if (volume.profile.TryGet<MotionBlur>(out motionBlur))
        {
            DOVirtual.Float(1f, 0f, duration, e =>
            {
                motionBlur.intensity.value = e;
            });
        }
        DepthOfField depthOfField;
        if (volume.profile.TryGet<DepthOfField>(out depthOfField))
        {
            DOVirtual.Float(0.3f, 3.5f, duration, e =>
            {
                depthOfField.focusDistance.value = e;
            });
            DOVirtual.Float(32, 18, duration, e =>
            {
                depthOfField.focalLength.value = e;
            });
        }

        await Task.Delay(Mathf.RoundToInt(duration) * 800);
        isFlashbang = false;
    }

    private void OnDestroy()
    {
        PlayerHealth.damageTaken -= DamageVignette;
        EntityHealth.enemyDeath -= ContrastPop;
        Flashbang.flashbangExplode -= FlashBangEffect;
        RandomizeGun.gunSelectGraphic -= GunSelectGraphic;
        GameManager.hasDied -= DeathSequence;
    }
}
