using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Currency Data", menuName = "GADE/Currency")]
public class CurrencyData : ScriptableObject
{
    [Header("Currency")]
    public int Currency;

    [Header("Defender Costs")] 
    public List<DefenderCard> defenders;
    
    public event Action<int> onCurrencyChanged;

    public void ResetCurrency()
    {
        Currency = 0;
    }
    
    public void UpdateCurrency(int amount)
    {
        Currency += amount;
        onCurrencyChanged?.Invoke(Currency);
    }

    public bool CanPurchaseCannon()
    {
        var cardCost = defenders.FirstOrDefault(defenderCard => defenderCard.DefenderType == EDefender.Cannon).Cost;
        var canPurchase = Currency >= cardCost;
        return canPurchase;
    }
}

public enum EDefender
{
    Cannon,
    Archer,
}

[Serializable]
public struct DefenderCard
{
    public EDefender DefenderType;
    public int Cost;
}