using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RandomizeGun : MonoBehaviour
{
    [SerializeField] private Transform mysteryObjectsTransform;
    [SerializeField] private GameObject[] mysteryObjects;
    [SerializeField] private GameObject[] guns;
    [SerializeField] private int cycleCount = 16;
    [SerializeField] private AnimationCurve timerCurve;
    [SerializeField] private AudioClip selectSound;
    private AudioSource audioSource;
    private int mysteryIndex, currentCount, currentIndex;
    private float timerAdd;

    public static Action<AudioClip> playSelectSound;
    public static Action gunSelected;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        MoneyManager.activateSwitch += Randomizer;
    }

    private async void Randomizer()
    {
        await Task.Delay(1700);
        audioSource.Play();
        mysteryIndex = UnityEngine.Random.Range(0, mysteryObjects.Length);
        CycleMysteryObjects();
        mysteryObjectsTransform.localPosition = Vector3.zero;
        mysteryObjectsTransform.DOLocalMoveY(-80, 3).SetEase(Ease.OutQuad);
        DOVirtual.Float(0, 200, 2.5f, e => { timerAdd = e; }).SetEase(timerCurve);
    }

    private float timer = 100;
    private async void CycleMysteryObjects()
    {
        if (currentCount >= cycleCount)
        {
            if (currentIndex == mysteryIndex)
            {
                mysteryIndex = (mysteryIndex + 1) % mysteryObjects.Length;
                SelectMysteryObject();
            }
            audioSource.Stop();
            playSelectSound?.Invoke(selectSound);
            SelectGun(mysteryIndex);
            currentCount = 0;
            timer = 100;
            return;
        }

        mysteryIndex = (mysteryIndex + 1) % mysteryObjects.Length;
        SelectMysteryObject();
        await Task.Delay(Mathf.RoundToInt(timer));
        currentCount++;
        timer += timerAdd;
        CycleMysteryObjects();
    }

    private void SelectMysteryObject()
    {
        for (int i = 0; i < mysteryObjects.Length; i++)
        {
            mysteryObjects[i].SetActive(i == mysteryIndex);
        }
    }

    private async void SelectGun(int index)
    {
        await Task.Delay(500);
        gunSelected?.Invoke();
        currentIndex = index;
        for (int i = 0; i < guns.Length; i++)
        {
           guns[i].SetActive(i == index);
        }

        foreach (GameObject g in mysteryObjects) g.SetActive(false);

    }

    private void OnDestroy()
    {
        MoneyManager.activateSwitch -= Randomizer;
    }
}
