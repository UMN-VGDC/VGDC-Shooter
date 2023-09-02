using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{

    [SerializeField] private GameObject[] menuObjects;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private RectTransform scoreboardItemPos, panel;
    [SerializeField] private CanvasGroup group;
    private void Start()
    {
        startMenu.SetActive(true);
        GetComponent<HTTPManager>().GetData("", 0);
    }
    public void StartGame()
    {
        DOVirtual.Float(1, 0, 0.7f, e =>
        {
            group.alpha = e;
        });
        panel.DOAnchorPosX(-369, 0.7f).SetEase(Ease.InBack);
        scoreboardItemPos.DOAnchorPosX(-52, 0.7f).SetEase(Ease.InBack);

        GameManager.Instance.UpdateGameState(GameState.TutorialStage);
        startMenu.SetActive(false);
    }

    public void SettingsMenu() => SetMenu(1);
    public void CreditsMenu() => SetMenu(2);
    public void ExitMenu() => SetMenu(3);
    public void Back() => SetMenu(0);
    private void SetMenu(int index)
    {
        for (int i = 0; i < menuObjects.Length; i++)
        {
            menuObjects[i].SetActive(i == index);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
