using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip timerAlert;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PatrolBot.patrolCharge += PlayTargetAlert;
        PatrolBot.patrolShoot += PlaySound;
        Fishie.fishieTargets += PlayTargetAlert;
    }

    private void PlayTargetAlert()
    {
        audioSource.PlayOneShot(timerAlert);
    }
    private void PlaySound(AudioClip audioClip) => audioSource.PlayOneShot(audioClip);

    private void OnDestroy()
    {
        PatrolBot.patrolCharge -= PlayTargetAlert;
        PatrolBot.patrolShoot -= PlaySound;
        Fishie.fishieTargets -= PlayTargetAlert;
    }
}
