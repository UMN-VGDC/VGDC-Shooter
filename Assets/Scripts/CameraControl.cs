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

    [SerializeField] private Transform crossHair;
    [SerializeField] private float lookSensitivity = 50f;
    [SerializeField] TagTracker tracker;

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

    void UpdateCursor(Vector2 screenPos)
    {
        Vector2 cursorPosOffset = screenPos - new Vector2(0.5f, 0.5f);
        float rotX = cursorPosOffset.y * -lookSensitivity;
        float rotY = cursorPosOffset.x * lookSensitivity;
        transform.localRotation = Quaternion.Euler(rotX, rotY, 0f);
    }

    

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = tracker.GetDampedScreenPos();
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
