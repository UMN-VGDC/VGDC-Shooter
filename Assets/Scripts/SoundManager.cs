using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] playerDamageSounds;
    [SerializeField] private AudioClip critSound, bulletImpact, enemyHit, shootSound, waterSplashSound;

    private static AudioSource audioSource;
    private QueueSound critQueue, shootQueue, enemyHitQueue, waterSplashQueue;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerHealth.damageTaken += PlayerDamage;
        Bullet.crit += AddCritQueue;
        EntityHealth.enemyHit += AddEnemyHitQueue;
        Shoot.shootBullet += AddShootQueue;
        EnemyDeath.deathSound += EnemyDeathSound;
        EntityHealth.deathSound += EnemyDeathSound;
        WaterSplash.splashSound += AddWaterSplashQueue;

        critQueue = new QueueSound(critSound, 100);
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
        audioSource.PlayOneShot(playerDamageSounds[Random.Range(0, playerDamageSounds.Length)]);
    }
    public static void PlaySound(AudioClip clip) => audioSource.PlayOneShot(clip);
    private void AddWaterSplashQueue() => waterSplashQueue.SoundQueue();
    private void AddCritQueue() => critQueue.SoundQueue();
    private void AddShootQueue() => shootQueue.SoundQueue();
    private void AddEnemyHitQueue() => enemyHitQueue.SoundQueue();

    private void OnDestroy()
    {
        PlayerHealth.damageTaken -= PlayerDamage;
        Bullet.crit -= AddCritQueue;
        EntityHealth.enemyHit -= AddEnemyHitQueue;
        Shoot.shootBullet -= AddShootQueue;
        EnemyDeath.deathSound -= EnemyDeathSound;
        EntityHealth.deathSound -= EnemyDeathSound;
        WaterSplash.splashSound -= AddWaterSplashQueue;
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
