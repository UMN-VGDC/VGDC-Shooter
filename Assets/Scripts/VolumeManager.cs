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
    private bool isFlashbang;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.damageTaken += DamageVignette;
        EntityHealth.enemyDeath += ContrastPop;
        Flashbang.flashbangExplode += FlashBangEffect;
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
        if (i == 0) return;
        Bloom bloom;
        if (volume.profile.TryGet<Bloom>(out bloom))
        {
            DOVirtual.Float(2f, 1f, 0.4f, e =>
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
            DOVirtual.Float(0f, 1f, 0.4f, e =>
            {
                colorAdjustmnets.postExposure.value = e;
            });
            
        }

    }

    private async void FlashBangEffect()
    {
        float duration = 4f;
        isFlashbang = true;

        ColorAdjustments colorAdjustmnets;
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustmnets))
        {
            DOVirtual.Float(5.2f, 1f, duration, e =>
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
            DOVirtual.Float(0.52f, 3.5f, duration, e =>
            {
                depthOfField.focusDistance.value = e;
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
    }
}
