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
        CardUIHandler.OnCardPicked += HandleCardPicked;
        CardUIHandler.OnUpgradeCalled += HandleUpgradeCalled;
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
        //
        Ray mousePos = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(mousePos, out var defenderHit, 100f, defenderLayer))
        {
            Debug.Log($"{defenderHit.transform.name} has been hit");
            if (defenderHit.transform.TryGetComponent<UpgradeDefender>( out var upgrade))
            {
                Debug.Log("Upgrade Defender");
                upgrade.Upgrade();
            }

            _upgrading = false;
            
        }

        if(!canPlace) return;
        if (Physics.Raycast(mousePos, out var hit, 100f))
        {
            Debug.Log(hit.transform.tag + " || " + hit.transform.position);
            
            if (!hit.transform.CompareTag("Ground") || !(hit.point.y < 2)) return;
            
            //Do something
            Debug.Log($"Trying to place on {hit.transform.tag}");
            currencyData.UpdateCurrency(-_card.Cost);
            SoundManager.Instance.PlaySound(SoundType.CannonPlaced);
            SpawnDefender(hit.point);
            canPlace = false;
            onTowerPlaced?.Invoke();
        }
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

