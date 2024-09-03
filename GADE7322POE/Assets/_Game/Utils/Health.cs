using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    public int MaxHealth = 100;
    public DamageType damageType;
    public GameObject parentObject;

    private int _currentHealth;
    private Animator _animator;
    
    public UnityEvent<float> onDamaged;
    public static event Action<float, DamageType> onHealthDamaged;
    public static event Action onGameEnd;

    private void Start()
    {
        _currentHealth = MaxHealth;
        _animator = GetComponent<Animator>();
    }

    public void Damage(int amount)
    {
        _currentHealth -= amount;
        onDamaged?.Invoke(GetHealthFillAmount());
        onHealthDamaged?.Invoke((float)_currentHealth / ((float)MaxHealth/ 100f), damageType);

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
            SoundManager.Instance.PlaySound(SoundType.EnemyKilled);
            yield return new WaitForSeconds(2f);
        }

        yield return new WaitForSeconds(0.25f);
        Destroy(parentObject);
    }
    
    public void Death()
    {
        if(damageType == DamageType.Tower) onGameEnd?.Invoke();
        StartCoroutine(DeathAnim());
    }

    private float GetHealthFillAmount()
    {
        return (float)_currentHealth / (float)MaxHealth;
    }
}

public enum DamageType
{
    Tower,
    Goblin,
    Defender
}
