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
    
    private Queue<Vector3> pathQueue;
    private Queue<GameObject> defenders;
    
    private Vector3 offset;
    private Vector3 currentPos;
    private Vector3 nextPos;
    
    private float speed;
    
    private NavMeshAgent agent;
    private Animator _animator;
    
    private bool spawned;
    
    private bool attacking;
    private float timer;

    private void OnEnable()
    {
        pathQueue = new Queue<Vector3>();
        defenders = new Queue<GameObject>();
        spawned = false;
        timer = attackTimer;

        _animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }

    public void InitEnemy(List<Vector3> path, Vector3 offset, float speed)
    {
        foreach (var point in path)
        {
            pathQueue.Enqueue(point);
        }
        
        this.offset = offset;
        this.speed = speed;

        transform.position = path[0];
        nextPos = path[0];
        spawned = true;
    }

    private void Update()
    {
        if(!spawned) return;

        if (defenders.Count > 0)
        {
            AttackDefender();
        }
        else
        {
            FollowPath();
        }
    }

    private void FollowPath()
    {
        if(pathQueue.Count == 0) return;
        
        var direction = nextPos - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
        
        if (HasReachedDestination(nextPos))
        {
            nextPos = pathQueue.Dequeue();
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, nextPos, Time.deltaTime * speed);
        }
    }

    private bool HasReachedDestination(Vector3 nextPos)
    {
        var distance = Vector3.Distance(transform.position, nextPos);
        return distance < 1;
    }

    private void AttackDefender()
    {
        Debug.Log("Attack");

        var defender = defenders.Peek();
        
        if (defender == null)
        {
            _animator.SetBool("Attack", false);
            _animator.SetBool("RUN", true);
            
            defenders.Dequeue();
            return;
        }

        if (!attacking)
        {
            _animator.SetBool("RUN", false);
            _animator.SetBool("Attack", true);
            attacking = true;
        }
        
        var distance = Vector3.Distance(transform.position, defender.transform.position);
        transform.position = Vector3.Lerp(transform.position, defender.transform.position, Time.deltaTime * 1.1f);
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if(defender.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Damage(attackDamage);
            }

            timer = attackTimer;
        }


    }

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Defender"))
        {
            defenders.Enqueue(other.gameObject);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Defender"))
        {
            if (defenders.Contains(other.gameObject))
            {
                defenders.Enqueue(other.gameObject);
            }

            if (defenders.Count == 0)
            {
                attacking = false;
            }
        }
    }
}

public interface IState
{
    public void OnEnter();
    public void OnUpdate();
    public void OnExit();
}

public class AttackState : IState
{
    private Animator anim;
    private float attackRate;
    private GameObject target;

    private float timer;


    public AttackState(Animator anim, GameObject target, float attackRate)
    {
        this.anim = anim;
        this.target = target;
        this.attackRate = attackRate;

        timer = attackRate;
    }

    public void OnEnter()
    {
        anim.SetBool("RUN", false);
        anim.SetBool("Attack", true);
    }

    public void OnUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = attackRate;
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Damage(10);
            }
            
        }
        
        
    }

    public void OnExit()
    {
        anim.SetBool("RUN", true);
        anim.SetBool("Attack", false);
    }
}
