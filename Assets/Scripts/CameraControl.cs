using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CameraControl : MonoBehaviour
{
    private CinemachineVirtualCamera m_Camera;
    private CinemachinePOV pov;
    private CinemachineBasicMultiChannelPerlin m_BasicMultiChannelPerlin;
    private float amplitude, frequency;

    [SerializeField] private Transform crossHair;
    [SerializeField] private float lookSensitivity = 50f;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<CinemachineVirtualCamera>();
        pov = m_Camera.GetCinemachineComponent<CinemachinePOV>();
        m_BasicMultiChannelPerlin = m_Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        amplitude = m_BasicMultiChannelPerlin.m_AmplitudeGain;
        frequency = m_BasicMultiChannelPerlin.m_FrequencyGain;

        PlayerHealth.damageTakenAmount += DamageShake;
        EnemyDeath.killPoints += KillShake;
    }

    private void DamageShake(int intensity)
    {
        int damp = intensity / 100;
        DOVirtual.Float(1f, 0f, 1f, e =>
        {
            m_BasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(amplitude, 4f * damp, e);
            m_BasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(frequency, 20f * damp, e);
        });
    }

    private void KillShake(int intensity)
    {
        int damp = 1 + (intensity / 1000);
        DOVirtual.Float(1f, 0f, 0.6f, e =>
        {
            m_BasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(amplitude, 3f * damp, e);
            m_BasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(frequency, 10f * damp, e);
        });
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 cursorPosOffset = cursorPos - new Vector2(0.5f, 0.5f);
        float rotX = cursorPosOffset.y * -lookSensitivity;
        float rotY = cursorPosOffset.x * lookSensitivity;
        transform.localRotation = Quaternion.Euler(rotX, rotY, 0f);
    }

    private void OnDestroy()
    {
        PlayerHealth.damageTakenAmount -= DamageShake;
        EnemyDeath.killPoints -= KillShake;
    }
}
