using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private CinemachineVirtualCamera m_Camera;
    private CinemachinePOV pov;
    private CinemachineBasicMultiChannelPerlin m_BasicMultiChannelPerlin;
    private float amplitude, frequency;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<CinemachineVirtualCamera>();
        pov = m_Camera.GetCinemachineComponent<CinemachinePOV>();
        m_BasicMultiChannelPerlin = m_Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        amplitude = m_BasicMultiChannelPerlin.m_AmplitudeGain;
        frequency = m_BasicMultiChannelPerlin.m_FrequencyGain;

        Cursor.lockState = CursorLockMode.Locked;
        EnableCameraLookEase();

        PlayerHealth.damageTakenAmount += DamageShake;
        EnemyDeath.killPoints += KillShake;
    }

    private void DamageShake(int intensity)
    {
        DOVirtual.Float(1f, 0f, 1f, e =>
        {
            m_BasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(amplitude, 2.5f * intensity, e);
            m_BasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(frequency, 6f * intensity, e);
        });
    }

    private void KillShake(int intensity)
    {
        DOVirtual.Float(1f, 0f, 0.3f, e =>
        {
            m_BasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(amplitude, 2.5f * intensity, e);
            m_BasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(frequency, 6f * intensity, e);
        });
    }

    private async void EnableCameraLookEase()
    {
        await Task.Delay(2000);
        Cursor.lockState = CursorLockMode.None;
        pov.m_HorizontalRecentering.m_enabled = false;
        pov.m_VerticalRecentering.m_enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        PlayerHealth.damageTakenAmount -= DamageShake;
        EnemyDeath.killPoints -= KillShake;
    }
}
