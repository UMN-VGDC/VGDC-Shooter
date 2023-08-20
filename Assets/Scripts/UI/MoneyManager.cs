using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] coin;
    [SerializeField] private Image UISwitchBar;
    [SerializeField] private RectTransform SwitchBarContainer, switchIcon;
    [SerializeField] private AudioClip switchBarFullSound, hatchSound;
    [SerializeField] private int moneyRequirement = 40;
    [SerializeField] private Animator hatchAnimator;
    [SerializeField] private CanvasGroup glowGroup;
    [SerializeField] private Transform[] glowImages;

    private float currentFill, currentMoney;
    private bool isFilled;

    private Camera mainCamera;
    private Transform itemDropsParent;

    public static Action<AudioClip> activateSwitchSound;
    public static Action activateSwitch;
    public static Action<AudioClip> playSwitchBarFullSound;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        DropMoney.dropMoney += InstantiateMoney;
        MoneyUIAnimation.moneyAdd += FillSwitchBar;
        itemDropsParent = GameObject.FindGameObjectWithTag("Item Drops").transform;
        UISwitchBar.fillAmount = 0;

        glowImages[0].DOLocalRotate(new Vector3(0, 0, 360), 5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        glowImages[1].DOLocalRotate(new Vector3(0, 0, -360), 8f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    private void FillSwitchBar(int amount)
    {
        if (currentFill == 1) return;
        currentMoney += amount;
        currentFill = currentMoney / moneyRequirement;
        DOVirtual.Float(UISwitchBar.fillAmount, currentFill, 0.5f, e =>
        {
            if (isFilled) return;
            UISwitchBar.fillAmount = e;
        }).SetEase(Ease.OutQuad);
        DOTween.Kill(SwitchBarContainer);
        if (currentFill >= 1)
        {
            isFilled = true;
            UISwitchBar.fillAmount = 1;
            currentFill = 1;
            float scale = 3f;
            SwitchBarContainer.localScale = new Vector3(scale, scale, scale);
            SwitchBarContainer.DOScale(new Vector3(1, 1, 1), 2f);
            SwitchWeaponSequence();
            switchIcon.DORotate(new Vector3(0, 0, 360), 1.5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
            activateSwitch?.Invoke();
            playSwitchBarFullSound?.Invoke(switchBarFullSound);
            return;
        }
        else
        {
            float scale = 1.5f;
            SwitchBarContainer.localScale = new Vector3(scale, scale, scale);
            SwitchBarContainer.DOScale(new Vector3(1, 1, 1), 2f);
        }
    }

    private async void SwitchWeaponSequence()
    {
        await Task.Delay(1000);
        activateSwitchSound?.Invoke(hatchSound);
        DOVirtual.Float(0f, 1f, 2f, e => glowGroup.alpha = e);
        hatchAnimator.SetTrigger("Open");
        await Task.Delay(4000);
        hatchAnimator.SetTrigger("Close");
        isFilled = false;
        currentFill = 0f;
        currentMoney = 0f;
        DOVirtual.Float(1f, 0f, 0.5f, e => UISwitchBar.fillAmount = e).SetEase(Ease.OutQuad);
        DOVirtual.Float(1f, 0f, 1.5f, e => glowGroup.alpha = e);
        DOTween.Kill(switchIcon);
        switchIcon.DORotate(Vector3.zero, 1f);
    }

    private void InstantiateMoney(Transform pos, int amount)
    {
        List<int> coins = SelectCoins(amount);
        for (int i = 0; i < coins.Count; i++)
        {
            //Debug.Log(coins[i]);
            int index = 0;
            switch(coins[i])
            {
                case 1: index = 0; break;
                case 2: index = 1; break;
                case 4: index = 2; break;
            }
            GameObject coinInstance = Instantiate(coin[index], pos.position, itemDropsParent.rotation);
            coinInstance.transform.parent = itemDropsParent;
            coinInstance.transform.localRotation = Quaternion.Euler(90, UnityEngine.Random.Range(-270, 270), 0);
            Vector3 cameraPos = mainCamera.WorldToScreenPoint(coinInstance.transform.position);
            coinInstance.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(cameraPos.x, cameraPos.y, 1f));
        }
    }

    private int[] moneyValues = new int[] { 1, 2, 4 };
    private List<int> SelectCoins(int amount)
    {
        int currentValue = 0;
        List<int> answer = new List<int>();
        int size = moneyValues.Length;
        float[] probabilities = new float[] { 1, 24, 75 };

        while (currentValue < amount && size > 0)
        {

            int maxValue = currentValue + moneyValues[size - 1];
            if (maxValue > amount)
            {
                probabilities[size - 2] += probabilities[size - 1];
                size -= 1;
                continue;
            }
            int value = Probability(moneyValues, probabilities, size);
            currentValue += value;
            answer.Add(value);
        }
        return answer;
    }


    private int Probability(int[] values, float[] probabilities, int size)
    {
        float r = UnityEngine.Random.Range(0, 100);
        float cumulative = 0f;
        for (int i = 0; i < size; i++)
        {
            cumulative += probabilities[i];
            if (r < cumulative)
            {
                int select = values[i];
                return select;
            }

        }
        return -1;
    }

    private void OnDestroy()
    {
        DropMoney.dropMoney -= InstantiateMoney;
        MoneyUIAnimation.moneyAdd -= FillSwitchBar;
        DOTween.Kill(switchIcon);
        foreach (Transform t in glowImages) DOTween.Kill(t);
    }
}
