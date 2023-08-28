using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip[] playerDamageSounds;
    [SerializeField] private AudioSource enemyAudioSource, moneyAudioSource, newWeaponAudioSource, deathAudioSource, deathMusicAudioSource;
    [SerializeField] private AudioClip critSound, defaultShootSound, enemyHit, waterSplashSound;
    [Header("Streak")]
    [SerializeField] private AudioSource streakAudioSource;
    [SerializeField] private AudioClip scoreStreakSound, carRevSound;
    [SerializeField] private AudioClip[] familyVoice;

    private static AudioSource audioSource;
    private QueueSound shootQueue, enemyHitQueue, waterSplashQueue;

    public static Action flameThrower;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerHealth.damageTaken += PlayerDamage;
        EntityHealth.enemyHit += AddEnemyHitQueue;
        Shoot.shootBullet += AddShootQueue;
        Shoot.setShootSound += ReplaceShootSound;
        EnemyDeath.deathSound += EnemyDeathSound;
        WaterSplash.splashSound += AddWaterSplashQueue;
        ScoreStreakManager.scoreStreak += PlayStreakSound;
        MoneyManager.playSwitchBarFullSound += SwitchBarFull;
        MoneyManager.activateSwitchSound += PlaySound;
        Missile.missileLaunchSound += PlaySound;
        Flashbang.flashbangExplode += FlashbangMuffle;
        MoneyUIAnimation.playMoneySound += PlayMoneySound;
        RandomizeGun.playSelectSound += PlaySound;
        RandomizeGun.playGunSelectSound += GunSelectGraphicQuiet;
        GameManager.hasDied += PlayDeathSound;

        enemyHitQueue = new QueueSound(enemyHit, 100);
        waterSplashQueue = new QueueSound(waterSplashSound, 70);
        shootQueue = new QueueSound(defaultShootSound, 100);

        audioMixer.SetFloat("CombatVolume", 0);
        audioMixer.SetFloat("CombatLowpass", 22000);
        audioMixer.SetFloat("NewWeaponVolume", 0);
    }

    private void ReplaceShootSound(AudioClip clip)
    {
        shootQueue.delayedAudioClip = clip;
    }

    private void EnemyDeathSound(AudioClip[] clip)
    {
        if (clip.Length == 0) return;
        foreach (AudioClip c in clip) audioSource.PlayOneShot(c);
    }

    private void PlayerDamage()
    {
        audioSource.PlayOneShot(playerDamageSounds[UnityEngine.Random.Range(0, playerDamageSounds.Length)]);
    }
    private async void PlayStreakSound()
    {
        streakAudioSource.PlayOneShot(scoreStreakSound);
        DOVirtual.Float(.4f, 1f, 4f, e =>
        {
            audioMixer.SetFloat("PlayerEnemyPitch", e);
        });
        await Task.Delay(1000);
        AudioClip randomSound = familyVoice[UnityEngine.Random.Range(0, familyVoice.Length)];
        streakAudioSource.PlayOneShot(randomSound);
        await Task.Delay((int)Math.Round(randomSound.length * 1000) - 300);
        streakAudioSource.PlayOneShot(carRevSound);
        flameThrower?.Invoke();
    }

    private void GunSelectGraphicQuiet(AudioClip clip)
    {
        newWeaponAudioSource.PlayOneShot(clip);
        Sequence s = DOTween.Sequence();
        s.Append(DOVirtual.Float(0, -11, 0.6f, e => audioMixer.SetFloat("CombatVolume", e))).SetUpdate(true);
        s.Append(DOVirtual.Float(-11, 0, 0.6f, e => audioMixer.SetFloat("CombatVolume", e)).SetDelay(1f)).SetUpdate(true);
    }

    private void FlashbangMuffle()
    {
        DOVirtual.Float(2400f, 22000f, 7f, e =>
        {
            audioMixer.SetFloat("PlayerEnemyLowpass", e);
        });
    }

    private void SwitchBarFull(AudioClip clip)
    {
        PlayMoneySound(clip);
        //lower volume of Player & Enemy Sounds
        DOVirtual.Float(-9.4f, 0f, 2f, e =>
        {
            audioMixer.SetFloat("PlayerEnemyVolume", e);
        });
    }

    private void PlayDeathSound()
    {
        deathAudioSource.Play();
        deathMusicAudioSource.Play();
        audioMixer.SetFloat("CombatVolume", -13f);
        audioMixer.SetFloat("CombatLowpass", 2400);
        audioMixer.SetFloat("NewWeaponVolume", -13f);
    }
    public static void PlaySound(AudioClip clip) => audioSource.PlayOneShot(clip);
    private void PlayMoneySound(AudioClip clip) => moneyAudioSource.PlayOneShot(clip);
    private void AddWaterSplashQueue() => waterSplashQueue.SoundQueue();
    private void AddShootQueue() => shootQueue.SoundQueue();
    private void AddEnemyHitQueue() => enemyHitQueue.SoundQueue();

    private void OnDestroy()
    {
        PlayerHealth.damageTaken -= PlayerDamage;
        EntityHealth.enemyHit -= AddEnemyHitQueue;
        Shoot.shootBullet -= AddShootQueue;
        Shoot.setShootSound -= ReplaceShootSound;
        EnemyDeath.deathSound -= EnemyDeathSound;
        WaterSplash.splashSound -= AddWaterSplashQueue;
        ScoreStreakManager.scoreStreak -= PlayStreakSound;
        MoneyManager.playSwitchBarFullSound -= SwitchBarFull;
        MoneyManager.activateSwitchSound -= PlaySound;
        Missile.missileLaunchSound -= PlaySound;
        Flashbang.flashbangExplode -= FlashbangMuffle;
        MoneyUIAnimation.playMoneySound -= PlayMoneySound;
        RandomizeGun.playSelectSound -= PlaySound;
        RandomizeGun.playGunSelectSound -= GunSelectGraphicQuiet;
        GameManager.hasDied -= PlayDeathSound;
    }
}

public class QueueSound
{
    public AudioClip delayedAudioClip;
    private int audioDelay;
    private bool isPlaying;
    public QueueSound(AudioClip clip, int delay)
    {
        delayedAudioClip = clip;
        audioDelay = delay;
    }

    public async void SoundQueue()
    {
        if (isPlaying) return;
        isPlaying = true;
        await Task.Delay(audioDelay);
        SoundManager.PlaySound(delayedAudioClip);
        isPlaying = false;
    }
}
