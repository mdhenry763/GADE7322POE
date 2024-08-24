using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDefence : MonoBehaviour, IDamageable
{
    [Header("Cannon Elements")]
    public Transform cannon;
    public Transform shootPos;
    public GameObject cannonBall;

    [Header("Cannon Shoot Settings")] 
    public float shootTimer;
    private bool _canShoot;
    private float _timer;
    
    private List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        _canShoot = false;
        _timer = shootTimer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("enemyBug")) return;
        enemies.Add(other.gameObject);
        
    }

    private void Update()
    {
        RotateTowardsClosestEnemy();
        
        //Shoot Timer
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            _canShoot = true;
            _timer = shootTimer;
        }
    }

    void RotateTowardsClosestEnemy()
    {
        if(enemies.Count == 0) return;
    
        // Calculate direction from cannon to the enemy
        var direction = GetClosestEnemyDirection();
    
        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
    
        // Smoothly rotate the cannon towards the target
        cannon.rotation = Quaternion.Slerp(cannon.rotation, targetRotation, Time.deltaTime * 5);
        
        Shoot(direction);
    }

    void Shoot(Vector3 direction)
    {
        if (_canShoot)
        {
            var spawnedBall = Instantiate(cannonBall, shootPos.position, Quaternion.identity);
            if (spawnedBall.TryGetComponent<CannonBallLogic>(out var cannonBallLogic))
            {
                cannonBallLogic.InitMotion(enemies[0].transform.position - cannon.position);
            }
            _canShoot = false;
        }
    }
    
    private Vector3 GetClosestEnemyDirection()
    {
        Vector3 closestPosition = Vector3.positiveInfinity;
        float minDistance = float.MaxValue;
        
        //loop through all enemies and get closest enemy then return direction
        foreach (var enemy in enemies)
        {
            var distance = Vector3.Distance(cannon.position, enemy.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestPosition = enemy.transform.position;
            }
        }
        
        return closestPosition - cannon.position;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("enemyBug")) return;
        enemies.Remove(other.gameObject);
    }

    public void Damage(int amount)
    {
        
    }
}

public interface IDamageable
{
    public void Damage(int amount);
}