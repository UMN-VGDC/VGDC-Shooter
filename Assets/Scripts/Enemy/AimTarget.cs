using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class AimTarget : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerTargetBreak;
    [SerializeField] private UnityEvent triggerTargetTimeout;
    [SerializeField] private GameObject target;
    [SerializeField] private EntityHealth entityHealth;
    [SerializeField] private TextMeshPro textMeshPro;

    public void TriggerTargetBreak()
    {
        triggerTargetBreak?.Invoke();
        target.SetActive(false);
    }

    public void TriggerTargetTimeout()
    {
        triggerTargetTimeout?.Invoke();
        target.SetActive(false);
    }

    public void EnableTarget()
    {
        target.SetActive(true);
        entityHealth.ResetHealth();
    }

    public void DisableTarget()
    {
        target.SetActive(false);
    }

    public void SetText(string text)
    {
        textMeshPro.SetText(text);
    }
}
