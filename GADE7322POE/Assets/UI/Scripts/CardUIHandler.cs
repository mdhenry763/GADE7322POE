using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class CardUIHandler : UtkBase
{
    public CurrencyData currencyData;

    private VisualElement _mTowerHealthBar;
    
    private Label _mPlaceLbl;
    private Label _mCurrencyLbl;
    private Label _mCoinLbl;

    private Button _mPauseBtn;
    private Button _mCannonBtn;

    public UnityEvent onCannonSelected;
    public UnityEvent onPaused;
    
    
    protected override void Start()
    {
        base.Start();
        
        _mTowerHealthBar = rootElement.Q<VisualElement>("HealthBar");
        _mTowerHealthBar.style.width = new Length(100, LengthUnit.Percent);
        
        _mCurrencyLbl = rootElement.Q<Label>("CurrencyLbl");
        _mCoinLbl = rootElement.Q<Label>("CoinLbl");
        _mCurrencyLbl.text = "50";
        
        _mCannonBtn = rootElement.Q<Button>("CannonBtn");
        _mPauseBtn = rootElement.Q<Button>("PauseBtn");
        _mPauseBtn.clicked += HandlePauseLogic;
        _mCannonBtn.clicked += HandleCannonLogic;
        
        _mPlaceLbl = rootElement.Q<Label>("PlaceLabel");
        
        currencyData.onCurrencyChanged += HandleCurrencyChange;
        Health.onHealthDamaged += UpdateTowerHealthBar;

    }

    private int _paused = 0;
    void HandlePauseLogic()
    {
        SoundManager.Instance.PlaySound(SoundType.Button);
        onPaused?.Invoke();
        Time.timeScale = 0;
    }

    private void UpdateTowerHealthBar(float value, DamageType type)
    {
        if(type != DamageType.Tower) return;
        SoundManager.Instance.PlaySound(SoundType.TowerDamaged);
        _mTowerHealthBar.style.width = new Length(value, LengthUnit.Percent);
    }

    private Coroutine _coinCoroutine;
    private void HandleCurrencyChange(int value)
    {
        if(_coinCoroutine != null) StopCoroutine(_coinCoroutine);
        _coinCoroutine = StartCoroutine(CoinHide());
        _mCurrencyLbl.text = value.ToString();
    }

    IEnumerator CoinHide()
    {
        _mCoinLbl.RemoveFromClassList("coin_hide");
        yield return new WaitForSeconds(1f);
        _mCoinLbl.AddToClassList("coin_hide");
    }
    
    private void HandleCannonLogic()
    {
        SoundManager.Instance.PlaySound(SoundType.Button);
        _mPlaceLbl.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        if (currencyData.CanPurchaseCannon())
        {
            Debug.Log("Picked Cannon");
            onCannonSelected?.Invoke();
        }
        else
        {
            _mPlaceLbl.text = "You do not have enough gold!";
            StartCoroutine(HideText());
        }
        
    }

    IEnumerator HideText()
    {
        yield return new WaitForSeconds(2f);
        _mPlaceLbl.text = "Place cannon on terrain";
        _mPlaceLbl.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
    }

    public void HideCannonPrompt()
    {
        _mPlaceLbl.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
    }
}