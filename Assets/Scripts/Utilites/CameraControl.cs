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
    
    private bool isDead;

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
        isDead = false;
        PlayerHealth.damageTakenAmount += DamageShake;
        EntityHealth.enemyDeath += KillShake;
        Flashbang.flashbangExplode += FlashbangShake;
        GameManager.hasDied += HasDied;
        GameManager.restartGame += RestartGame;
    }

    private void HasDied() => isDead = true;

    private void RestartGame() => isDead = false;

    private void DamageShake(int intensity)
    {
        int damp = intensity / 100;

        Shake(damp * 4f, damp * 20f, 1f);
    }

    private void KillShake(int intensity)
    {
        if (intensity == 0) return;
        float damp = 1 + (intensity / 1000);
        if (intensity >= 2500) damp = 3.5f;
        Shake(damp * 3f, damp * 10f, 0.6f);
    }

    private void FlashbangShake()
    {
        Shake(6.5f, 10f, 5f);
    }

    private void Shake(float amp, float freq, float duration)
    {
        if (isDead) return;
        DOVirtual.Float(1f, 0f, duration, e =>
        {
            m_BasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(amplitude, amp, e);
            m_BasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(frequency, freq, e);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isDead) return;
        Vector2 cursorPos = CameraReciever.instance.GetDampedScreenPos();
        //Debug.Log("Cursor Pos: " + cursorPos);
        Vector2 cursorPosOffset = cursorPos - new Vector2(0.5f, 0.5f);
        float rotX = cursorPosOffset.y * -lookSensitivity;
        float rotY = cursorPosOffset.x * lookSensitivity;
        transform.localRotation = Quaternion.Euler(rotX, rotY, 0f);
    }

    private void OnDestroy()
    {
        PlayerHealth.damageTakenAmount -= DamageShake;
        EntityHealth.enemyDeath -= KillShake;
        Flashbang.flashbangExplode -= FlashbangShake;
        GameManager.hasDied -= HasDied;
    }
}
