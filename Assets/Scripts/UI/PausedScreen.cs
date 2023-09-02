using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausedScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private RectTransform panel;
    private void Awake()
    {
        GameManager.paused += TransitionIn;
        GameManager.stateChange += TransitionOut;
    }

    private void TransitionIn()
    {
        group.blocksRaycasts = true;
        DOVirtual.Float(0, 1, 0.5f, e =>
        {
            group.alpha = e;
        }).SetUpdate(true);

        panel.DOAnchorPosY(-373, 0);
        panel.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    private void TransitionOut()
    {
        if (GameManager.Instance.getGameState() == GameState.Paused) return;
        if (group.alpha == 0) return;
        group.blocksRaycasts = false;
        DOVirtual.Float(1, 0, 0.5f, e =>
        {
            group.alpha = e;
        }).SetUpdate(true);

        panel.DOAnchorPosY(373, 0.5f).SetEase(Ease.InBack).SetUpdate(true);
    }

    public void Resume()
    {
        GameManager.Instance.PauseUnpause();
    }

    private void OnDestroy()
    {
        GameManager.paused -= TransitionIn;
        GameManager.stateChange -= TransitionOut;
    }
}
