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

    private void Start()
    {
        currencyData.ResetCurrency();
        currencyData.UpdateCurrency(50);
        currencyCoroutine = StartCoroutine(SetPlayerCurrency());
    }

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