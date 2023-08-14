using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioSource flashBangAudioSource;
    [SerializeField] private AudioClip timerAlert, timerHurrySound, timerTimeout, targetBreak;
    [SerializeField] private AudioClip bossShieldDown;
    [SerializeField] private AudioClip flashbangExplode, flashbangRing;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PatrolBot.patrolCharge += PlayTargetAlert;
        PatrolBot.patrolShoot += PlaySound;
        MultiTargetEnemy.multiTargetsTimeout += PlayTargetTimeout;
        MultiTargetEnemy.multiTargetsInit += PlayTargetAlert;
        MultiTargetEnemy.multiTargetsShieldDownSound += PlayBossShieldDown;
        AimTarget.playTargetBreakSound += PlaySound;
        Fishie.fishieShoot += PlaySound;
        MilitaryDrone.militaryDroneShootSound += PlaySound;
        Flashbang.flashbangExplode += PlayFlashBang;
        Crockie.crockieRoar += PlaySound;
        UIManager.warningSound += PlaySound;
    }

    private void PlayTargetTimeout() => audioSource.PlayOneShot(timerTimeout);
    private void PlayTargetAlert() => audioSource.PlayOneShot(timerAlert);
    private void PlayBossShieldDown() => audioSource.PlayOneShot(bossShieldDown);
    private void PlayFlashBang()
    {
        flashBangAudioSource.PlayOneShot(flashbangExplode);
        flashBangAudioSource.PlayOneShot(flashbangRing);
    }

    private void PlaySound(AudioClip audioClip) => audioSource.PlayOneShot(audioClip);

    private void OnDestroy()
    {
        PatrolBot.patrolCharge -= PlayTargetAlert;
        PatrolBot.patrolShoot -= PlaySound;
        MultiTargetEnemy.multiTargetsTimeout -= PlayTargetTimeout;
        MultiTargetEnemy.multiTargetsInit -= PlayTargetAlert;
        MultiTargetEnemy.multiTargetsShieldDownSound -= PlayBossShieldDown;
        AimTarget.playTargetBreakSound -= PlaySound;
        Fishie.fishieShoot -= PlaySound;
        MilitaryDrone.militaryDroneShootSound -= PlaySound;
        Crockie.crockieRoar -= PlaySound;
        UIManager.warningSound -= PlaySound;
    }
}
