using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    
    private Controls controls;
    private Camera _mainCam;

    private void Awake()
    {
        controls = new Controls();
        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void LateUpdate()
    {
        var input = controls.Player.Move.ReadValue<Vector2>();
        var camTransform = _mainCam.transform.position;

        camTransform.x += 1 * input.x * moveSpeed;
        camTransform.z += 1 * input.y * moveSpeed;

        _mainCam.transform.position = camTransform;
    }
}
