using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefenderHandler : MonoBehaviour
{
    public GameObject defender;
    
    private Camera _mainCam;
    private Controls _controls;

    private void Awake()
    {
        if (_controls == null)
        {
            _controls = new Controls();
        }
        
        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        _controls.Enable();
        
        _controls.Player.Fire.performed += MouseClick;
    }

    private void MouseClick(InputAction.CallbackContext obj)
    {
        RaycastHit hit;

        Ray mousePos = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(mousePos, out hit, 100f))
        {
            Debug.Log(hit.transform.tag + " || " + hit.transform.position);
            if (!hit.transform.CompareTag("Path") && hit.point.y < 2)
            {
                //Do something
                SpawnDefender(hit.point);
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
