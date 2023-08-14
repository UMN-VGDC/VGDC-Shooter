using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using System;

public class AimTarget : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerTargetBreak;
    [SerializeField] private UnityEvent triggerTargetTimeout;
    [SerializeField] private GameObject target;
    [SerializeField] private EntityHealth entityHealth;
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private ParticleSystem timeoutVFX;
    [SerializeField] private ParticleSystem timerVFX, timerBreakVFX;
    [SerializeField] private AudioClip targetBreakSound;

    public static Action<AudioClip> playTargetBreakSound;

    private Color textCol, transparentCol;
    private float textFontSize;
    private void Start()
    {
        textCol = textMeshPro.color;
        transparentCol = new Color(textCol.r, textCol.g, textCol.b, 0);
        textMeshPro.color = transparentCol;
        textFontSize = textMeshPro.fontSize;
    }
    public void TriggerTargetBreak()
    {
        triggerTargetBreak?.Invoke();
        target.SetActive(false);
        textMeshPro.color = transparentCol;
        timerBreakVFX.Play();
        playTargetBreakSound?.Invoke(targetBreakSound);
    }

    public void TriggerTargetTimeout()
    {
        triggerTargetTimeout?.Invoke();
        target.SetActive(false);
        textMeshPro.color = transparentCol;
        timeoutVFX.Play();
    }

    public void EnableTarget()
    {
        if (gameObject == null) return;
        target.SetActive(true);
        entityHealth.ResetHealth();
        textMeshPro.DOColor(textCol, 1f);
        DOVirtual.Float(textFontSize * 4, textFontSize, 0.7f, e =>
        {
            textMeshPro.fontSize = e;
        }).SetEase(Ease.OutExpo);
    }

    public void DisableTarget()
    {
        target.SetActive(false);
    }

    public void SetText(string text)
    {
        textMeshPro.SetText(text);
    }

    public float GetParticleLifetime()
    {
        ParticleSystem.MainModule main = timerVFX.main;
        return main.startLifetime.constant;
    }
}
