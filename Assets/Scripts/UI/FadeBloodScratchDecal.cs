using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBloodScratchDecal : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        RandomizeGun.gunSelectGraphic += FadeDecal;
        image = GetComponent<Image>();
        image.DOColor(new Color(0, 0, 0, 0), fadeDuration).SetDelay(0.5f).onComplete = () => Destroy(gameObject);
    }

    private void FadeDecal()
    {
        DOTween.Kill(image);
        image.DOColor(new Color(0, 0, 0, 0), 0.3f).onComplete = () => Destroy(gameObject);
    }

    private void OnDestroy()
    {
        RandomizeGun.gunSelectGraphic -= FadeDecal;
        DOTween.Kill(image);
    }
}
