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
    [SerializeField] private AudioSource enemyAudioSource;
    [SerializeField] private AudioClip critSound, bulletImpact, enemyHit, shootSound, waterSplashSound;
    [Header("Streak")]
    [SerializeField] private AudioSource streakAudioSource;
    [SerializeField] private AudioClip scoreStreakSound, carRevSound;
    [SerializeField] private AudioClip[] familyVoice;

    private static AudioSource audioSource;
    private QueueSound critQueue, shootQueue, enemyHitQueue, waterSplashQueue;

    public static Action flameThrower;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerHealth.damageTaken += PlayerDamage;
        EntityHealth.enemyHit += AddEnemyHitQueue;
        Shoot.shootBullet += AddShootQueue;
        EnemyDeath.deathSound += EnemyDeathSound;
        WaterSplash.splashSound += AddWaterSplashQueue;
        UIManager.scoreStreak += PlayStreakSound;
        Missile.missileLaunchSound += PlaySound;
        Flashbang.flashbangExplode += FlashbangMuffle;

        shootQueue = new QueueSound(shootSound, 100);
        enemyHitQueue = new QueueSound(enemyHit, 100);
        waterSplashQueue = new QueueSound(waterSplashSound, 70);


    }

    private void EnemyDeathSound(AudioClip[] clip)
    {
        if (clip.Length == 0) return;
        foreach(AudioClip c in clip) audioSource.PlayOneShot(c);
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

    private void FlashbangMuffle()
    {
        DOVirtual.Float(2400f, 22000f, 7f, e =>
        {
            audioMixer.SetFloat("PlayerEnemyLowpass", e);
        });
    }

    public static void PlaySound(AudioClip clip) => audioSource.PlayOneShot(clip);
    private void AddWaterSplashQueue() => waterSplashQueue.SoundQueue();
    private void AddShootQueue() => shootQueue.SoundQueue();
    private void AddEnemyHitQueue() => enemyHitQueue.SoundQueue();

    private void OnDestroy()
    {
        PlayerHealth.damageTaken -= PlayerDamage;
        EntityHealth.enemyHit -= AddEnemyHitQueue;
        Shoot.shootBullet -= AddShootQueue;
        EnemyDeath.deathSound -= EnemyDeathSound;
        WaterSplash.splashSound -= AddWaterSplashQueue;
        UIManager.scoreStreak -= PlayStreakSound;
        Missile.missileLaunchSound -= PlaySound;
        Flashbang.flashbangExplode -= FlashbangMuffle;
    }
}

public class QueueSound
{
    private AudioClip audioClip;
    private int audioDelay;
    private bool isPlaying;
    public QueueSound(AudioClip clip, int delay)
    {
        audioClip = clip;
        audioDelay = delay;
    }

    public async void SoundQueue()
    {
        if (isPlaying) return;
        isPlaying = true;
        await Task.Delay(audioDelay);
        SoundManager.PlaySound(audioClip);
        isPlaying = false;
    }
}
