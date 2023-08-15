using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMoney : MonoBehaviour
{
    [SerializeField] private int amount = 3;
    [SerializeField] private Transform dropTransform;
    public static Action<Transform, int> dropMoney;
    public void DropCoins()
    {
        Transform pos = dropTransform == null ? transform : dropTransform;
        dropMoney?.Invoke(pos, amount);

    }
}
