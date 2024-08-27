using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    public int MaxHealth = 100;

    private int _currentHealth;
    private Animator _animator;
    
    public UnityEvent<float> onDamaged;
    public static event Action<float> onHealthDamaged; 

    private void Start()
    {
        _currentHealth = MaxHealth;
        _animator = GetComponent<Animator>();
    }

    public void Damage(int amount)
    {
        _currentHealth -= amount;
        onDamaged?.Invoke(GetHealthFillAmount());
        //onHealthDamaged?.Invoke((float)_currentHealth / ((float)MaxHealth/ 100f));

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    IEnumerator DeathAnim()
    {
        if (_animator)
        {
            _animator.SetBool("RUN", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Death", true);
        }

        yield return 1f;
        gameObject.SetActive(false);
    }
    
    public void Death()
    {
        StopCoroutine(DeathAnim());
    }

    private float GetHealthFillAmount()
    {
        return (float)_currentHealth / (float)MaxHealth;
    }
}
