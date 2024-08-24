using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    public int MaxHealth = 100;

    private int _currentHealth;
    public UnityEvent<float> onDamaged;

    private void Start()
    {
        _currentHealth = MaxHealth;
    }

    public void Damage(int amount)
    {
        _currentHealth -= amount;
        onDamaged?.Invoke(GetHealthFillAmount());

        if (_currentHealth <= 0)
        {
            Debug.Log(transform.name + " is dead");
        }
    }

    private float GetHealthFillAmount()
    {
        return (float)_currentHealth / (float)MaxHealth;
    }
}
