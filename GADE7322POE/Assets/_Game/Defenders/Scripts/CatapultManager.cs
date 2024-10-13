using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultManager : MonoBehaviour
{
    [Header("Spawn")] 
    public GameObject cannonBall;
    public float spawnTimer = 3f;
    public Transform spawnPos;
    
    [Header("Settings")] 
    public float minDistance = 5;
    
    private List<GameObject> _enemies = new();

    private float timer = 0;

    private void Start()
    {
        timer = spawnTimer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("enemyBug")) return;
        _enemies.Add(other.gameObject);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("enemyBug")) return;
        _enemies.Remove(other.gameObject);
    }

    public void Shoot()
    {
        Debug.Log("Shoot");
        if(_enemies.Count == 0) return;
        if (GetClosestEnemyPosition() == transform.position) return;
        
        var cannon = Instantiate(cannonBall);
        cannon.transform.position = spawnPos.position;
        
        var catapultProjectile = cannon.GetComponent<CatapultProjectile>();
        catapultProjectile.InitializeProjectile(spawnPos.position, GetClosestEnemyPosition());
        Debug.Log("Shoot double");
    }

    private Vector3 GetClosestEnemyPosition() //Returns closest distance
    {
        Vector3 closestPosition = transform.position;
        float minDistance = float.MaxValue;
        
        //loop through all enemies and get closest enemy then return direction
        foreach (var enemy in _enemies)
        {
            if (enemy == null)
            {
                _enemies.Remove(enemy);
                return closestPosition;
            }
            
            var distance = Vector3.Distance(transform.position, enemy.transform.position);

            //if shorter distance set closestPosition to distance
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPosition = enemy.transform.position;
            }
        }
        
        return closestPosition;
    }
}
