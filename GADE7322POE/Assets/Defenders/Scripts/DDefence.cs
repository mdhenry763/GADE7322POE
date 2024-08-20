using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDefence : MonoBehaviour
{
    public Transform cannon;
    
    private List<GameObject> enemies = new List<GameObject>();
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("enemyBug")) return;
        enemies.Add(other.gameObject);
        
    }

    private void Update()
    {
        if(enemies.Count == 0) return;
        var direction = transform.position - enemies[0].transform.position;
        cannon.forward = -direction;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("enemyBug")) return;
        enemies.Remove(other.gameObject);
    }
}
