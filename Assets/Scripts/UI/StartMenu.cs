using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{

    [SerializeField] private GameObject[] menuObjects;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private RectTransform scoreboardItemPos, panel;
    [SerializeField] private CanvasGroup group;

    [Header("Intro Video")]
    [SerializeField] private CanvasGroup videoGroup;
    [SerializeField] private TextMeshProUGUI startText;
    private bool isStartMenu = true;

    private void Start()
    {
        startMenu.SetActive(true);
        GetComponent<HTTPManager>().GetData("", 0);

        videoGroup.alpha = 1;
        Color col = startText.color;
        startText.DOColor(new Color(col.r, col.g, col.b, 0.1f), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
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

    public void ResetVideoScreen()
    {
        isStartMenu = true;
        videoGroup.blocksRaycasts = true;
        videoGroup.interactable = true;
        DOVirtual.Float(0, 1, 0.5f, e =>
        {
            videoGroup.alpha = e;
        });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isStartMenu) return;
            isStartMenu = false;
            videoGroup.blocksRaycasts = false;
            videoGroup.interactable = false;
            DOVirtual.Float(1, 0, 0.5f, e =>
            {
                videoGroup.alpha = e;
            });
        }
    }

    private void OnDestroy()
    {
        DOTween.Kill(startText);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
