using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable //Handle all game health
{
    public int MaxHealth = 100;
    public ParticleSystem damagePS;
    public DamageType damageType;
    public GameObject parentObject;

    private int _currentHealth;
    private Animator _animator;
    
    public UnityEvent<float> onDamaged;
    public static event Action<float, DamageType> onHealthDamaged;
    public static event Action onGameEnd;

    private void Start() //Set health and death anim for enemies
    {
        _currentHealth = MaxHealth;
        _animator = GetComponent<Animator>();
    }

    public void Damage(int amount) //Handle entity damage
    {
        _currentHealth -= amount;
        damagePS.Play(); //Play damage effect if there is one
        onDamaged?.Invoke(GetHealthFillAmount());
        onHealthDamaged?.Invoke((float)_currentHealth / ((float)MaxHealth/ 100f), damageType);

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    IEnumerator DeathAnim() //Play Death anim then destroy object
    {
        if (_animator)
        {
            _animator.SetBool("RUN", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Death", true);
            //Play death sound
            SoundManager.Instance.PlaySound(SoundType.EnemyKilled);
            yield return new WaitForSeconds(2f);
        }

        yield return new WaitForSeconds(0.25f);
        Destroy(parentObject);
    }
    
    public void Death() //Death after health = 0
    {
        if(damageType == DamageType.Tower) onGameEnd?.Invoke();
        StartCoroutine(DeathAnim());
    }

    private float GetHealthFillAmount() //For UI
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
