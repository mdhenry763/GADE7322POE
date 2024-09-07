using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Health health;
    public Image healthBar;

    //Set UI Health bar on objects
    public void SetHealthBar(float fillAmount)
    {
        healthBar.fillAmount = fillAmount;
    }
}
