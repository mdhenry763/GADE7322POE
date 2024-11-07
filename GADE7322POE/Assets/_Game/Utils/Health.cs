using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Health : MonoBehaviour, IDamageable //Handle all game health
{
    public int MaxHealth = 100;
    public ParticleSystem damagePS;
    public DamageType damageType;
    public GameObject parentObject;
    
    [Header("Animation")]
    public Animator animator;
    public string deathAnim;

    private int _currentHealth;
    private int _deathHashCode;
    
    [Space]
    public UnityEvent<float> onDamaged;
    public static event Action<float, DamageType> onHealthDamaged;
    public static event Action onGameEnd;
    public static event Action OnEnemyDied;

    private void Start() //Set health and death anim for enemies
    {
        _currentHealth = MaxHealth;
        animator = GetComponent<Animator>();
        _deathHashCode = Animator.StringToHash(deathAnim);

        if (damageType == DamageType.Tower)
        {
            DefendersController.AddDefender(gameObject);
        }
    }

    public void ResetHealth()
    {
        _currentHealth = MaxHealth;
        onDamaged?.Invoke(GetHealthFillAmount());
        onHealthDamaged?.Invoke((float)_currentHealth / ((float)MaxHealth/ 100f), damageType);
    }

    public void Damage(int amount) //Handle entity damage
    {
        _currentHealth -= amount;
        if (damagePS != null)
        {
            damagePS.Play(); //Play damage effect if there is one
        }
        onDamaged?.Invoke(GetHealthFillAmount());
        onHealthDamaged?.Invoke((float)_currentHealth / ((float)MaxHealth/ 100f), damageType);

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    IEnumerator DeathAnim() //Play Death anim then destroy object
    {
        if (animator)
        {
            animator.CrossFade(_deathHashCode, 0.1f);
            //Play death sound
            SoundManager.Instance.PlaySound(SoundType.EnemyKilled);
            yield return new WaitForSeconds(2f);
        }

        yield return new WaitForSeconds(0.25f);
        if (damageType is DamageType.Defender or DamageType.Tower)
        {
            DefendersController.RemoveDefender(gameObject);
        }

        if (damageType == DamageType.Goblin)
        {
            EnemyAmountHandler.RemoveEnemy(parentObject);
        }
        
        Destroy(parentObject);
    }
    
    public void Death() //Death after health = 0
    {
        switch (damageType)
        {
            case DamageType.Tower:
                onGameEnd?.Invoke();
                break;
            case DamageType.Goblin:
                OnEnemyDied?.Invoke();
                break;
        }

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
