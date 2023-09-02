using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ReadyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI readyText;
    private AudioSource audioSource;
    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        canvasGroup = GetComponent<CanvasGroup>();
        DummyTarget.dummyTargetsDestroyed += PlayAnimation;
        transform.localScale = new Vector3 (1f, 0f, 1f);
    }

    private async void PlayAnimation()
    {
        await Task.Delay(1000);
        DOVirtual.Float(0, 1, 0.3f, e =>
        {
            canvasGroup.alpha = e;
        });

        DOVirtual.Float(26, 62, 2, e =>
        {
            readyText.characterSpacing = e;
        });
        transform.DOScaleY(1f, 0.5f).SetEase(Ease.OutBack);
        audioSource.Play();

        await Task.Delay(1500);

        transform.DOScaleY(0f, 0.5f).SetEase(Ease.InBack);

        DOVirtual.Float(1, 0, 0.3f, e =>
        {
            canvasGroup.alpha = e;
        }).SetDelay(0.2f);
    }

    private void OnDestroy()
    {
        DummyTarget.dummyTargetsDestroyed -= PlayAnimation;
    }
}
