using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class CannonBallLogic : MonoBehaviour
{
    public int damageAmount = 10;
    public float speedMultiplier = 5f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyAfterLifetime());
    }

    public void InitMotion(Vector3 direction)
    {
        _rigidbody.velocity = direction * speedMultiplier;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Defender")) return;
        Debug.Log(other.tag);
        
        if(other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.Damage(damageAmount);
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(3.5f);
        if (gameObject)
        {
            Destroy(gameObject);
        }
    }
}