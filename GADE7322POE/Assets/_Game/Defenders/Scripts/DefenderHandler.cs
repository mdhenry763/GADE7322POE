using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DefenderHandler : MonoBehaviour
{
    public LayerMask defenderLayer;
    public GameObject defender;
    public CurrencyData currencyData;
    
    private Camera _mainCam;
    private Controls _controls;

    private bool canPlace;
    private bool _upgrading;
    private bool _special;

    public UnityEvent<Vector3> onSpawnSpecial;
    public UnityEvent onTowerPlaced;
    private DefenderCard _card;

    private void Awake()
    {
        _controls ??= new Controls();

        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        _controls.Enable();
        
        _controls.Player.Fire.performed += MouseClick;
        CardUIHandler.OnSpecialCalled += HandleSpecial;
        CardUIHandler.OnCardPicked += HandleCardPicked;
        CardUIHandler.OnUpgradeCalled += HandleUpgradeCalled;
    }

    private void HandleSpecial()
    {
        _special = true;
    }

    private void HandleCardPicked(DefenderCard card)
    {
        _card = card;
        defender = card.Prefab;
        canPlace = true;
    }

    private void HandleUpgradeCalled()
    {
        Debug.Log("Upgrade Called");
        _upgrading = true;
    }

    public void CanPlaceDefender()
    {
        //canPlace = true;
    }
    
    

    /// <summary>
    /// Spawn defender on mouse click after check if can spawn
    /// </summary>
    /// <param name="obj"></param>
    private void MouseClick(InputAction.CallbackContext obj)
    {
        Ray mousePos = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        UpgradeDefender(mousePos);

        
        if (Physics.Raycast(mousePos, out var hit, 100f))
        {
            Debug.Log(hit.transform.tag + " || " + hit.transform.position);
            
            if (!hit.transform.CompareTag("Ground") || !(hit.point.y < 2)) return;
            
            //Do something
            Debug.Log($"Trying to place on {hit.transform.tag}");
            
            if (_special)
            {
                Debug.Log("Place Ally");
                onSpawnSpecial?.Invoke(hit.point);
                _special = false;
                return;
            }
            
            if(!canPlace) return;
            currencyData.UpdateCurrency(-_card.Cost);
            SoundManager.Instance.PlaySound(SoundType.CannonPlaced);
            SpawnDefender(hit.point);
            canPlace = false;
            onTowerPlaced?.Invoke();
        }
    }

    private void UpgradeDefender(Ray mousePos)
    {
        if (!_upgrading) return;

        if (!Physics.Raycast(mousePos, out var defenderHit, 100f, defenderLayer)) return;
        
        Debug.Log($"{defenderHit.transform.name} has been hit");
        if (defenderHit.transform.TryGetComponent<UpgradeDefender>( out var upgrade))
        {
            Debug.Log("Upgrade Defender");
            upgrade.Upgrade();
            currencyData.UpdateCurrency(-100);
            onTowerPlaced?.Invoke();
        }

        _upgrading = false;
    }

    private void SpawnDefender(Vector3 position)
    {
        var spawnedDefender = Instantiate(defender, position, defender.transform.rotation);
        DefendersController.AddDefender(spawnedDefender);
    }

    private void OnDisable()
    {
        _controls.Disable();
        _controls.Player.Fire.performed -= MouseClick;
        CardUIHandler.OnCardPicked -= HandleCardPicked;
        CardUIHandler.OnUpgradeCalled -= HandleUpgradeCalled;
        DefendersController.ClearDefenders();
    }
    
}

