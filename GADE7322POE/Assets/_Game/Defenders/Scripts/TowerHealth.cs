using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TowerHealth : MonoBehaviour
{
    public Volume volume; // Assign your Volume in the Inspector
    public float maxIntensity = 0.5f; // Maximum vignette intensity at lowest health
    public Color damageColour = Color.red;
    private float initialIntensity;
    private Vignette _vignette;

    private void Start()
    {
        // Get the Vignette component from the Volume Profile
        if (volume.profile.TryGet<Vignette>(out _vignette))
        {
            initialIntensity = _vignette.intensity.value;
            _vignette.color = new ColorParameter(damageColour);
        }
    }

    private void OnEnable()
    {
        Health.onHealthDamaged += HandleTowerHealth;
    }

    private void HandleTowerHealth(float arg1, DamageType arg2)
    {
        if (arg2 == DamageType.Tower)
        {
            UpdateVignette(arg1);
        }
    }

    private void UpdateVignette(float healthPercentage)
    {
        // Calculate intensity based on health percentage
        float intensity = 1f/healthPercentage;
        Debug.Log($"Intensity: {intensity}");
        _vignette.intensity.value += intensity;
    }
}
