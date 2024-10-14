using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDefence : MonoBehaviour
{
    [Header("Cannon Elements")]
    public Transform cannon;
    public Transform shootPos;
    public GameObject cannonBall;
    public GameObject parent;

    [Header("Cannon Shoot Settings")] 
    public float shootTimer;
    private bool _canShoot;
    private float _timer;

    public bool oppositeDirection;
    
    private List<GameObject> enemies = new List<GameObject>();

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void Start()
    {
        _canShoot = false;
        _timer = shootTimer;
    }

    //Add enemy and remove enemy
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("enemyBug")) return;
        enemies.Add(other.gameObject);
        
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("enemyBug")) return;
        enemies.Remove(other.gameObject);
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
        Quaternion targetRotation = oppositeDirection
            ? Quaternion.LookRotation(direction.normalized)
            : Quaternion.LookRotation(-direction.normalized);
        
    
        // Smoothly rotate the cannon towards the target
        cannon.rotation = Quaternion.Slerp(cannon.rotation, targetRotation, Time.deltaTime * 5);
        
        Shoot(direction);
    }

    void Shoot(Vector3 direction) //Shoot enemy
    {
        if (!_canShoot) return;
        
        var spawnedBall = Instantiate(cannonBall, shootPos.position, Quaternion.identity);
        spawnedBall.transform.forward = -direction;
        if (spawnedBall.TryGetComponent<CannonBallLogic>(out var cannonBallLogic))
        {
            cannonBallLogic.InitMotion(direction);
            //SoundManager.Instance.PlaySound(SoundType.CannonFire);
        }

        
        _canShoot = false;
    }
    
    private Vector3 GetClosestEnemyDirection() //Returns closest distance
    {
        Vector3 closestPosition = Vector3.positiveInfinity;
        float minDistance = float.MaxValue;
        
        //loop through all enemies and get closest enemy then return direction
        foreach (var enemy in enemies)
        {
            if (enemy == null)
            {
                enemies.Remove(enemy);
                return Vector3.zero;
            }
            
            var distance = Vector3.Distance(cannon.position, enemy.transform.position);

            //if shorter distance set closestPosition to distance
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPosition = enemy.transform.position;
            }
        }
        
        return closestPosition - cannon.position;
    }

    public IDamageable GetParentObject()
    {
        return parent.GetComponent<IDamageable>();
    }

    private void OnDestroy()
    {
        if(gameObject == null) return;
        DefendersController.RemoveDefender(gameObject);
    }
}