using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GunSelectUIGraphic : MonoBehaviour
{
    [SerializeField] private GameObject[] mysteryObjects;
    [SerializeField] private GameObject[] revealObjects;
    [SerializeField] private CanvasGroup switchWeaponBackgroundGroup, vignette;
    [SerializeField] private Transform newGunTransform, mysteryObjectsTransform;
    [SerializeField] private new Light light;
    [SerializeField] private Transform glowImageLarge, glowImageSmall;
    [SerializeField] private Renderer blurRenderer;
    private int currentGunIndex;

    private void Start()
    {
        newGunTransform.localScale = Vector3.zero;
        glowImageLarge.localScale = Vector3.zero;
        glowImageSmall.localScale = Vector3.zero;
        RandomizeGun.gunSelectGraphic += SwitchWeapon;
        RandomizeGun.gunIndex += SelectMysteryObject;
        MoneyManager.activateSwitch += LowerMysteryObjects;
        newGunTransform.DOLocalRotate(new Vector3(0, 360, 0), 3f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetUpdate(true);
        glowImageLarge.DOLocalRotate(new Vector3(0, 0, 360), 6f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetUpdate(true);
        glowImageSmall.DOLocalRotate(new Vector3(0, 0, -360), 9f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetUpdate(true);
    }

    private async void LowerMysteryObjects()
    {
        await Task.Delay(1700);
        mysteryObjectsTransform.localPosition = Vector3.zero;
        mysteryObjectsTransform.DOLocalMoveY(-80, 3).SetEase(Ease.OutQuad);
    }

    private void SelectMysteryObject(int index)
    {
        if (index == -1)
        {
            foreach (GameObject g in mysteryObjects) g.SetActive(false);
            return;
        }

        currentGunIndex = index;

        for (int i = 0; i < mysteryObjects.Length; i++)
        {
            mysteryObjects[i].SetActive(i == index);
        }
    }

    private void SwitchWeapon()
    {
        for (int i = 0; i < revealObjects.Length; i++)
        {
            revealObjects[i].SetActive(i == currentGunIndex);
        }

        Sequence bg = DOTween.Sequence();
        bg.Append(DOVirtual.Float(0, 1, 0.6f, e => {
            switchWeaponBackgroundGroup.alpha = e;
            vignette.alpha = e;
        })).SetUpdate(true);
        bg.Append(DOVirtual.Float(1, 0, 0.6f, e => {
            switchWeaponBackgroundGroup.alpha = e;
            vignette.alpha = e;
        }).SetDelay(1f)).SetUpdate(true);

        Sequence model = DOTween.Sequence();
        model.Append(newGunTransform.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutBack)).SetUpdate(true);
        model.Append(newGunTransform.DOScale(Vector3.zero, 0.6f).SetDelay(1f).SetEase(Ease.InBack)).SetUpdate(true);

        //light sequence
        Sequence l = DOTween.Sequence();
        l.Append(DOVirtual.Float(0, 0.04f, 0.6f, e => light.intensity = e)).SetUpdate(true);
        l.Append(DOVirtual.Float(0.04f, 0, 0.6f, e => light.intensity = e).SetDelay(1f)).SetUpdate(true);

        //blur effect
        Sequence b = DOTween.Sequence();
        b.Append(DOVirtual.Float(0, 1, 0.6f, e => blurRenderer.material.SetFloat("_Alpha", e))).SetUpdate(true);
        b.Append(DOVirtual.Float(1, 0, 0.6f, e => blurRenderer.material.SetFloat("_Alpha", e)).SetDelay(1f)).SetUpdate(true);

        //glow large
        Sequence glowL = DOTween.Sequence();
        glowL.Append(glowImageLarge.DOScale(Vector3.one, 0.6f).SetDelay(0.4f).SetEase(Ease.OutBack)).SetUpdate(true);
        glowL.Append(glowImageLarge.DOScale(Vector3.zero, 0.6f).SetDelay(0.6f).SetEase(Ease.InBack)).SetUpdate(true);

        //glow small
        Sequence glowS = DOTween.Sequence();
        glowS.Append(glowImageSmall.DOScale(Vector3.one, 0.6f).SetDelay(0.2f).SetEase(Ease.OutBack)).SetUpdate(true);
        glowS.Append(glowImageSmall.DOScale(Vector3.zero, 0.6f).SetDelay(0.6f).SetEase(Ease.InBack)).SetUpdate(true);
    }

    private void OnDestroy()
    {
        RandomizeGun.gunSelectGraphic -= SwitchWeapon;
        RandomizeGun.gunIndex -= SelectMysteryObject;
        DOTween.Kill(newGunTransform);
        DOTween.Kill(glowImageLarge);
        DOTween.Kill(glowImageSmall);
    }
}
