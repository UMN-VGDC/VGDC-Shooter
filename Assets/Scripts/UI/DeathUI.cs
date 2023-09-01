using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup deathUIGroup, sendScoreGroup, bloodSplatterGroup, scoreboardGroup, backdropGroup;
    [SerializeField] private TextMeshProUGUI gameOverText, scoreText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject scoreScreen;
    [SerializeField] private Transform scoreboardTransform;
    [SerializeField] private float scrollSpeed = 5f;
    private int score;
    private bool isScrollUp;
    private bool isScrolling = true;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.hasDied += DeathSequence;
        ScoreboardItem.stopScrolling += StopScrolling;
    }

    private void StopScrolling() => isScrolling = false;

    private void DeathSequence()
    {
        DOVirtual.Float(0, 0.8f, 8, e =>
        {
            bloodSplatterGroup.alpha = e;

        }).SetEase(Ease.Linear).SetUpdate(true);
        DOVirtual.Float(0, 1, 3, e =>
        {
            deathUIGroup.alpha = e;
        }).SetDelay(1.5f).SetEase(Ease.Linear).SetUpdate(true);

        DOVirtual.Float(30, 70, 5, e =>
        {
            gameOverText.characterSpacing = e;
        }).SetDelay(1.5f).SetEase(Ease.Linear).SetUpdate(true);
        gameOverText.DOColor(new Color(0, 0, 0, 0), 1).SetDelay(5.5f).SetUpdate(true).OnComplete(() => ScoreScreen());
    }

    private void ScoreScreen()
    {
        sendScoreGroup.blocksRaycasts = true;
        DOVirtual.Float(0, 1, 1.5f, e =>
        {
            backdropGroup.alpha = e;
            sendScoreGroup.alpha = e;
        }).SetEase(Ease.Linear).SetUpdate(true);

        score = GetComponent<ScoreStreakManager>().GetScore();
        scoreScreen.SetActive(true);
        DOVirtual.Int(0, score, 2f, e =>
        {
            scoreText.text = e.ToString();
        }).SetDelay(1.5f).SetUpdate(true);
    }

    public void SubmitButton()
    {
        GetComponent<HTTPManager>().PostData(inputField.text, score);
        FadeScoreScreen();
    }

    private bool canScroll = true;
    public void SkipButton()
    {
        canScroll = false;
        FadeScoreScreen();
    }

    private void FadeScoreScreen()
    {
        sendScoreGroup.interactable = false;
        DOVirtual.Float(1, 0, 0.6f, e =>
        {
            sendScoreGroup.alpha = e;
        }).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => ScoreboardScreen());
    }

    private async void ScoreboardScreen()
    {
        scoreboardGroup.blocksRaycasts = true;
        GetComponent<HTTPManager>().GetData(inputField.text, score);
        DOVirtual.Float(0, 1, 1, e =>
        {
            scoreboardGroup.alpha = e;
        }).SetEase(Ease.Linear).SetUpdate(true);

        if (!canScroll) return;
        await Task.Delay(3000);
        isScrollUp = true;
    }

    private void Update()
    {
        if (!isScrollUp || !isScrolling) return;
        Vector3 pos = scoreboardTransform.position;
        scoreboardTransform.position = new Vector3(pos.x, pos.y + Time.deltaTime * scrollSpeed, pos.z);
    }

    //public void ResetScene()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}

    private void OnDestroy()
    {
        GameManager.hasDied += DeathSequence;
        ScoreboardItem.stopScrolling -= StopScrolling;
    }
}
