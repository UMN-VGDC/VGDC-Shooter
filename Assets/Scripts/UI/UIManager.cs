using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System;
using System.Threading.Tasks;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject[] bloodScratch;

    [Header("Warning Effect")]
    [SerializeField] private CanvasGroup warningGroup;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private AudioClip warningBossSound;
    private bool warningIsPlaying;

    private bool isDestroyed;
    public static Action<AudioClip> warningSound;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.damageTaken += spawnBloodScratch;
        Spawner.bossSpawned += BossWarningEffect;
    }

    private async void BossWarningEffect()
    {
        if (warningIsPlaying) return;
        warningIsPlaying = true;
        warningSound?.Invoke(warningBossSound);

        DOVirtual.Float(0, 1, 0.5f, e =>
        {
            warningGroup.alpha = e;
        });

        DOVirtual.Float(50, 90, 4f, e =>
        {
            warningText.characterSpacing = e;
        });

        await Task.Delay(3000);
        if (isDestroyed) return;
        DOVirtual.Float(1, 0, 0.5f, e =>
        {
            warningGroup.alpha = e;
        });

        await Task.Delay(3000);
        if (isDestroyed) return;
        warningIsPlaying = false;
    }

    private void spawnBloodScratch()
    {
        Vector2 ramdomPos = new Vector2(UnityEngine.Random.Range(-200f, 200f), UnityEngine.Random.Range(-150f, 50f));
        GameObject decal = bloodScratch[UnityEngine.Random.Range(0, bloodScratch.Length)];
        GameObject instantiatedDecal = Instantiate(decal, Vector2.zero, Quaternion.identity, canvas.transform);
        RectTransform rt = instantiatedDecal.GetComponent<RectTransform>();
        rt.anchoredPosition = rt.transform.position;
        rt.localPosition = ramdomPos;

        int flip = UnityEngine.Random.value < 0.5f ? 1 : -1;
        Vector2 initialScale = instantiatedDecal.transform.localScale;
        instantiatedDecal.transform.localScale = new Vector2(initialScale.x * flip, initialScale.y);
    }

    private void OnDestroy()
    {
        PlayerHealth.damageTaken -= spawnBloodScratch;
        Spawner.bossSpawned -= BossWarningEffect;
        isDestroyed = true;
    }
}
