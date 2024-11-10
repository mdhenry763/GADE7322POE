using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class CardUIHandler : UtkBase
{
    public CurrencyData currencyData;

    public DefenderCard cannon;
    public DefenderCard archer;
    public DefenderCard crossbow;

    private VisualElement _mTowerHealthBar;
    private VisualElement _mRoundText;
    
    private Label _mPlaceLbl;
    private Label _mCurrencyLbl;
    private Label _mCoinLbl;
    private Label _mWaveLbl;
    private Label _mRoundLbl;
    private Label _mNewLbl;

    private Button _mPauseBtn;
    private Button _mCannonBtn;
    private Button _mArcherBtn;
    private Button _mCrossbowBtn;
    private Button _mUpgradeBtn;
    private Button _mSpecialBtn;

    private Coroutine roundTextShow;
    
    private int _paused = 0;

    public UnityEvent onCannonSelected;
    public UnityEvent onPaused;

    public static event Action<DefenderCard> OnCardPicked;
    public static event Action OnUpgradeCalled;
    public static event Action OnSpecialCalled;


    protected override void Start()
    {
        base.Start();
        
        //Setup Buttons and Labels for UI control
        _mTowerHealthBar = rootElement.Q<VisualElement>("HealthBar");
        _mRoundText = rootElement.Q<VisualElement>("RoundText");
        _mTowerHealthBar.style.width = new Length(100, LengthUnit.Percent);
        
        _mCurrencyLbl = rootElement.Q<Label>("CurrencyLbl");
        _mCoinLbl = rootElement.Q<Label>("CoinLbl");
        _mCurrencyLbl.text = "50";

        _mWaveLbl = rootElement.Q<Label>("WaveLabel");
        _mRoundLbl = rootElement.Q<Label>("RoundLabel");
        _mNewLbl = rootElement.Q<Label>("NewLbl");
        
        //Card buttons
        _mCannonBtn = rootElement.Q<Button>("CannonBtn");
        _mArcherBtn = rootElement.Q<Button>("ArcherBtn");
        _mCrossbowBtn = rootElement.Q<Button>("CrossbowBtn");
        _mUpgradeBtn = rootElement.Q<Button>("UpgradeBtn");
        _mSpecialBtn = rootElement.Q<Button>("SpecialBtn");
        
        _mPauseBtn = rootElement.Q<Button>("PauseBtn");
        
        //Handle onClick events
        _mPauseBtn.clicked += HandlePauseLogic;
        
        _mCannonBtn.clicked += HandleCannonPicked;
        _mArcherBtn.clicked += HandleArcherPicked;
        _mCrossbowBtn.clicked += HandleCrossbowPicked;
        _mUpgradeBtn.clicked += HandleUpgradeRequest;

        _mSpecialBtn.clicked += HandleSpecial;
        
        _mPlaceLbl = rootElement.Q<Label>("PlaceLabel");
        
        //Handle currency and Tower health
        currencyData.onCurrencyChanged += HandleCurrencyChange;
        Health.onHealthDamaged += UpdateTowerHealthBar;

    }

    private void OnEnable()
    {
        EnemyWaveController.OnWaveIncreased += HandleWaveIncrease;
        EnemyWaveController.OnRoundIncreased += HandleRoundIncrease;
    }

    private void OnDisable()
    {
        EnemyWaveController.OnWaveIncreased -= HandleWaveIncrease;
        EnemyWaveController.OnRoundIncreased -= HandleRoundIncrease;
    }

    private void HandleWaveIncrease(int wave)
    {
        _mNewLbl.text = "New wave";
        _mRoundText.RemoveFromClassList("coin_hide");
        _mWaveLbl.text = $"Wave: {wave}/5";
        if (roundTextShow != null)
        {
            StopCoroutine(roundTextShow);
        }
        roundTextShow = StartCoroutine(RoundTextShow());
    }

    private void HandleSpecial()
    {
        OnSpecialCalled?.Invoke();
        ShowPlaceText("Click on area to place special");
        StartCoroutine(HideText());
        //_mSpecialBtn.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
    }

    IEnumerator RoundTextShow()
    {
        yield return new WaitForSeconds(2f);
        _mRoundText.AddToClassList("coin_hide");
    }
    
    private void HandleRoundIncrease(int round)
    {
        _mRoundLbl.text = $"Round: {round}";
        _mNewLbl.text = "New round";
        _mRoundText.RemoveFromClassList("coin_hide");
        if (roundTextShow != null)
        {
            StopCoroutine(roundTextShow);
        }
        roundTextShow = StartCoroutine(RoundTextShow());
        _mSpecialBtn.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
    }

    private void HandleUpgradeRequest()
    {
        Debug.Log("Upgrade selected");

        if (currencyData.Currency >= 100)
        {
            ShowPlaceText("Click on a defender to upgrade!");
            OnUpgradeCalled?.Invoke();
        }
        else
        {
            ShowPlaceText("You do not have enough gold!");
            StartCoroutine(HideText());
        }
        
        
    }

    private void HandleCannonPicked()
    {
        HandleCardPicked(cannon);
    }

    private void HandleArcherPicked()
    {
        HandleCardPicked(archer);
    }

    private void HandleCrossbowPicked()
    {
        HandleCardPicked(crossbow);
    }
    
    void HandlePauseLogic()
    {
        SoundManager.Instance.PlaySound(SoundType.Button);
        onPaused?.Invoke();
        Time.timeScale = 0; //Stop game time
    }

    private void UpdateTowerHealthBar(float value, DamageType type)
    {
        if(type != DamageType.Tower) return;
        //Play Sound when tower damaged
        SoundManager.Instance.PlaySound(SoundType.TowerDamaged);
        //Set Health bar
        _mTowerHealthBar.style.width = new Length(value, LengthUnit.Percent);
    }

    private Coroutine _coinCoroutine;
    private void HandleCurrencyChange(int value) // Change coin label on UI 
    {
        if (_coinCoroutine != null)
        {
            StopCoroutine(_coinCoroutine);
        }
        _coinCoroutine = StartCoroutine(CoinHide());
        _mCurrencyLbl.text = value.ToString();
    }

    IEnumerator CoinHide() // Hide coin animation
    {
        _mCoinLbl.RemoveFromClassList("coin_hide");
        yield return new WaitForSeconds(1f);
        _mCoinLbl.AddToClassList("coin_hide");
    }
    
    private void HandleCardPicked(DefenderCard card) //handle cannon clicked
    {
        SoundManager.Instance.PlaySound(SoundType.Button);
        ShowPlaceText($"Place {card.DefenderType} on terrain");
        if (currencyData.CanPurchaseDefender(card))
        {
            Debug.Log($"Picked {card.DefenderType}");
            onCannonSelected?.Invoke();
            OnCardPicked?.Invoke(card);
        }
        else
        {
            _mPlaceLbl.text = "You do not have enough gold!";
            StartCoroutine(HideText());
        }
        
    }

    private void ShowPlaceText(string text)
    {
        _mPlaceLbl.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        _mPlaceLbl.text = text;
    }

    IEnumerator HideText()
    {
        yield return new WaitForSeconds(2f);
        _mPlaceLbl.text = "Place cannon on terrain";
        _mPlaceLbl.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
    }

    public void HideCannonPrompt() //Hide cannon propmt
    {
        _mPlaceLbl.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
    }

    private void OnDestroy()
    {
        _coinCoroutine = null;
        currencyData.onCurrencyChanged -= HandleCurrencyChange;
        Health.onHealthDamaged -= UpdateTowerHealthBar;
    }
}