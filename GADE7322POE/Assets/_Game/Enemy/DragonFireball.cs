using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFireball : MonoBehaviour
{
    [SerializeField] private int damageAmt = 5;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3 offset = new Vector3(0, 0.5f, 0);
    [SerializeField] private float force = 5;

    public void InitializeFireball(Vector3 direction)
    {
        rb.velocity = (direction + offset) * force;
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Defender"))
        {
            var health = other.GetComponent<IDamageable>();
            health.Damage(damageAmt);
            Destroy(gameObject);
        }
    }
    
}
