using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DefenderHandler : MonoBehaviour
{
    public GameObject defender;
    
    private Camera _mainCam;
    private Controls _controls;

    private bool canPlace = false;

    public UnityEvent onTowerPlaced;

    private void Awake()
    {
        _controls ??= new Controls();

        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        _controls.Enable();
        
        _controls.Player.Fire.performed += MouseClick;
    }

    public void CanPlaceDefender()
    {
        canPlace = true;
    }

    private void MouseClick(InputAction.CallbackContext obj)
    {
        if(!canPlace) return;
        Ray mousePos = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(mousePos, out var hit, 100f))
        {
            Debug.Log(hit.transform.tag + " || " + hit.transform.position);
            if (!hit.transform.CompareTag("Path") && hit.point.y < 2 && !hit.transform.CompareTag("Defender"))
            {
                //Do something
                
                SpawnDefender(hit.point);
                canPlace = false;
                onTowerPlaced?.Invoke();
            }
        }
    }

    private void SpawnDefender(Vector3 position)
    {
        Instantiate(defender, position, Quaternion.identity);
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    void Update()
    {
        
    }
}
