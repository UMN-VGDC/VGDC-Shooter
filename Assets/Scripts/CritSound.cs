using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class CritSound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip critSound;
    private bool isPlaying;
    private int comboCount;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Bullet.crit += SoundQueue;
        Bullet.crit += ComboPitch;
    }

    private async void ComboPitch()
    {
        comboCount++;
        audioSource.pitch = 0.8f + comboCount * 0.02f;
        int currentCombo = comboCount;
        await Task.Delay(500);
        if (currentCombo == comboCount)
        {
            audioSource.pitch = 0.8f;
            comboCount = 0;
        }
    }

    private async void SoundQueue()
    {
        if (isPlaying) return;
        isPlaying = true;
        await Task.Delay(80);
        audioSource.PlayOneShot(critSound);
        isPlaying = false;
    }

    private void OnDestroy()
    {
        Bullet.crit -= SoundQueue;
        Bullet.crit -= ComboPitch;
    }
}
