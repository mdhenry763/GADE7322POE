using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootAnim : MonoBehaviour
{
    public UnityEvent onShoot;
    
    public void Shoot()
    {
        onShoot?.Invoke();
    }
}
