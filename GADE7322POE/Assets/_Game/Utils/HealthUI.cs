using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Health health;
    public Image healthBar;

    public void SetHealthBar(float fillAmount)
    {
        healthBar.fillAmount = fillAmount;
    }
}
