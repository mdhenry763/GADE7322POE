using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Health health;
    public Image healthBar;
    public AimConstraint AimConstraint;

    private void OnEnable()
    {
        if (AimConstraint == null) return;
        
        var source = new ConstraintSource
        {
            sourceTransform = Camera.main.transform
        };
        AimConstraint.AddSource(source);
    }

    //Set UI Health bar on objects
    public void SetHealthBar(float fillAmount)
    {
        healthBar.fillAmount = fillAmount;
    }
}
