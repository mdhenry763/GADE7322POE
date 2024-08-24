using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public float attackTimer = 3f;
    public float attackDistance = 3f;
    public int attackDamage = 10;
    
    private List<Vector3> path;
    private Vector3 offset;
    private float speed;

    private int currentPathIndex;
    private NavMeshAgent agent;

    private Coroutine followPath;
    private Coroutine attack;

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }

    public void InitEnemy(List<Vector3> path, Vector3 offset, float speed)
    {
        this.path = path;
        this.offset = offset;
        this.speed = speed;

        transform.position = path[0];
        
        followPath = StartCoroutine(FollowPath(gameObject, 0));
    }
    
    IEnumerator FollowPath(GameObject enemy, int index)
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = index + 1; i < path.Count; i++)
        {
            var currentPosition = path[i - 1] + offset;
            currentPathIndex = i+1;
            var nextPosition = path[i] + offset;

            float elapsedTime = 0f;
            float journeyLength = Vector3.Distance(currentPosition, nextPosition);

            while (elapsedTime < journeyLength / speed)
            {
                // Calculate the interpolation factor (t)
                float t = elapsedTime * speed / journeyLength;

                // Interpolate position
                enemy.transform.position = Vector3.Lerp(currentPosition, nextPosition, t);
                
                //Set Direction
                var direction = nextPosition - enemy.transform.position;
                enemy.transform.rotation = Quaternion.LookRotation(direction);

                // Increase the elapsed time
                elapsedTime += Time.deltaTime;

                yield return null; // Wait for the next frame
            }
            
            // Ensure the enemy reaches the exact position
            enemy.transform.position = nextPosition;
        }
    }

    IEnumerator Attack(GameObject defender)
    {
        while (true)
        {
            var distance = Vector3.Distance(transform.position, defender.transform.position);

            if (distance <= attackDistance)
            {
                if(defender == null) StopAttacking();
                
                if (defender.TryGetComponent<IDamageable>(out var damageable))
                {
                    //Break out of attack if defender dead
                    damageable.Damage(attackDamage);
                }

                if (!defender.activeInHierarchy)
                {
                    StopAttacking();
                }

                yield return new WaitForSeconds(attackTimer);
            }
            yield return null;
        }
    }

    private GameObject attackingDefender;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Defender"))
        {
            //Stop path walk to defender
            StopCoroutine(followPath);
            agent.enabled = true;
            attackingDefender = other.gameObject;
            agent.SetDestination(other.transform.position);
            attack = StartCoroutine(Attack(other.gameObject));
        }
    }

    private void StopAttacking()
    {
        StopCoroutine(attack);
        agent.enabled = false;
        followPath = StartCoroutine(FollowPath(gameObject, currentPathIndex));
    }
}
