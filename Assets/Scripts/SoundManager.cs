using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip critSound;
    private AudioSource audioSource;

    private int CritSoundQueue;
    private bool isCritSoundPlaying;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Bullet.crit += PlayCritSound;
    }

    void PlayCritSound()
    {
        if (CritSoundQueue < 2) CritSoundQueue++;
    }

    // Update is called once per frame
    void Update()
    {
        if (CritSoundQueue > 0 && !isCritSoundPlaying)
        {
            PlayHitSound();
            isCritSoundPlaying = true;
        }
    }

    private async void PlayHitSound()
    {
        CritSoundQueue--;
        audioSource.PlayOneShot(critSound);
        await Task.Delay(100);
        isCritSoundPlaying = false;
    }

    private void OnDestroy()
    {
        Bullet.crit -= PlayCritSound;
    }
}
