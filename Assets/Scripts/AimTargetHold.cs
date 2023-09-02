using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class AimTargetHold : MonoBehaviour
{
    [SerializeField] private ParticleSystem failEffect, successEffect, target;
    [SerializeField] private EntityHealth entityHealth;
    private bool isSuccess;
    public static Action targetHoldSuccess;

    public void TargetHoldSuccess()
    {
        targetHoldSuccess?.Invoke();
        isSuccess = true;
        successEffect.Play();
        DecreaseHealth();
    }

    private async void DecreaseHealth()
    {
        await Task.Delay(200);
        if (entityHealth.GetCurrentHealth() == 0) return;
        entityHealth.DecreaseHealth(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.getGameState() == GameState.StartMenu) return;
        if (other.CompareTag("Gun Aim Target"))
        {
            target.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameManager.Instance.getGameState() == GameState.StartMenu) return;
        if (other.CompareTag("Gun Aim Target") && !isSuccess)
        {
            target.gameObject.SetActive(false);
            failEffect.Play();
        }
    }
}
