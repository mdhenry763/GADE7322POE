using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Currency Data", menuName = "GADE/Currency")]
public class CurrencyData : ScriptableObject
{
    [Header("Currency")]
    public int Currency;
    
    public event Action<int> onCurrencyChanged;

    public void ResetCurrency()
    {
        Currency = 0;
    }
    /// <summary>
    /// Update currency amount
    /// </summary>
    /// <param name="amount"></param>
    
    public void UpdateCurrency(int amount)
    {
        Currency += amount;
        onCurrencyChanged?.Invoke(Currency);
    }

    /// <summary>
    /// Handles purchasing of the cannon
    /// </summary>
    /// <returns></returns>
    public bool CanPurchaseDefender(DefenderCard card)
    {
        var cardCost = card.Cost;
        var canPurchase = Currency >= cardCost;
        return canPurchase;
    }
}

public enum EDefender
{
    Cannon,
    Archer,
    Catapult,
    Xbow
}

[Serializable]
public struct DefenderCard
{
    public EDefender DefenderType;
    public int Cost;
    public GameObject Prefab;
}