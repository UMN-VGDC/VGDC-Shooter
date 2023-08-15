using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] coin;

    private Camera mainCamera;
    private Transform itemDropsParent;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        DropMoney.dropMoney += InstantiateMoney;
        itemDropsParent = GameObject.FindGameObjectWithTag("Item Drops").transform;
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
            coinInstance.transform.localRotation = Quaternion.Euler(90, Random.Range(-270, 270), 0);
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
        float r = Random.Range(0, 100);
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
    }
}
