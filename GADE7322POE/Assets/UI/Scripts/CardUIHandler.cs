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

    private Button _mPauseBtn;
    private Button _mCannonBtn;

    public UnityEvent onCannonSelected;
    
    
    private void Start()
    {
        _mTowerHealthBar = rootElement.Q<VisualElement>("HealthBar");
        _mTowerHealthBar.style.width = new Length(100, LengthUnit.Percent);
        
        _mCurrencyLbl = rootElement.Q<Label>("CurrencyLbl");
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
        Debug.Log("Pause Game");
        _paused = _paused == 0 ? 1 : 0;
        Time.timeScale = _paused;
    }

    private void UpdateTowerHealthBar(float value)
    {
        _mTowerHealthBar.style.width = new Length(value, LengthUnit.Percent);
    }

    private void HandleCurrencyChange(int value)
    {
        _mCurrencyLbl.text = value.ToString();
    }

    private void HandleCannonLogic()
    {
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