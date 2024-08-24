using System;
using UnityEngine;

public class CannonBallLogic : MonoBehaviour
{
    public int DamageAmount = 10;
    public float speedMultiplier = 5f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
            damageable.Damage(DamageAmount);
            Destroy(gameObject);
        }
    }
}