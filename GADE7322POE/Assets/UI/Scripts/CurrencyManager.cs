using System;
using System.Collections;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int startCurrency = 50;
    public int currencyIncrease = 10;
    public float currencyIncreaseTimer = 10;
    
    [Header("References: ")]
    public CurrencyData currencyData;

    private Coroutine currencyCoroutine;

    private void Start() // Start coroutine for updating currency
    {
        currencyData.ResetCurrency();
        currencyData.UpdateCurrency(startCurrency);
        currencyCoroutine = StartCoroutine(SetPlayerCurrency());
    }

    private void OnEnable()
    {
        EnemyWaveController.OnRoundIncreased += HandleRoundIncrease;
    }

    void HandleRoundIncrease(int round)
    {
        if (round == 3)
        {
            currencyIncreaseTimer--;
        }
    }

    /// <summary>
    /// Handle Updating currency at interval
    /// </summary>
    /// <returns></returns>
    IEnumerator SetPlayerCurrency()
    {
        while (true)
        {
            yield return new WaitForSeconds(currencyIncreaseTimer);
            SoundManager.Instance.PlaySound(SoundType.CoinIncrease);
            currencyData.UpdateCurrency(currencyIncrease);
        }
    }

    private void OnDestroy()
    {
        if(currencyCoroutine != null) StopCoroutine(currencyCoroutine);
    }
}