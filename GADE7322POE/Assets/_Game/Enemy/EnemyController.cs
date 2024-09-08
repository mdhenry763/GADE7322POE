using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour //Script for handling enemy behaviour
{
    public float attackTimer = 3f;
    public float attackDistance = 3f;
    public int attackDamage = 10;
    public LayerMask defenderLayer;
    
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
        //Setup variables
        pathQueue = new Queue<Vector3>();
        defenders = new Queue<GameObject>();
        spawned = false;
        timer = attackTimer;

        _animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }

    public void InitEnemy(List<Vector3> path, Vector3 offset, float speed) //Initialise the enemy
    {
        foreach (var point in path)
        {
            pathQueue.Enqueue(point);
        }
        
        this.offset = offset;
        this.speed = speed;

        //Setup starting pos and next pos
        transform.position = path[0];
        nextPos = path[0];
        nextPos.y = 0.5f;
        spawned = true;
    }

    private void Update()
    {
        if(!spawned) return;

        //Check if there are any enemies to attack
        if (defenders.Count > 0)
        {
            AttackDefender();
        }
        else
        {
            speed = 1;
            FollowPath();
        }
    }

    private void FollowPath()
    {
        if(pathQueue.Count == 0) return;
        
        //Get & Set target direction 
        var direction = nextPos - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
        
        if (HasReachedDestination(nextPos))
        {
            //Change position if reached desired destination
            nextPos = pathQueue.Dequeue();
            nextPos.y = 0.5f;
        }
        else
        {
            //Move the enemy along the path
            transform.position = Vector3.Lerp(transform.position, nextPos, Time.deltaTime * speed);
        }
    }

    private bool HasReachedDestination(Vector3 nextPos) //Check if player has reached destination
    {
        var distance = Vector3.Distance(transform.position, nextPos);
        return distance < 1;
    }

    private void AttackDefender() //Attack defender obj
    {
        Debug.Log("Attack");

        //Get defender position
        var defender = defenders.Peek();
        if (defender == null)
        {
            //Reset animations to walking
            _animator.SetBool("Attack", false);
            _animator.SetBool("RUN", true);
            
            defenders.Dequeue();
            return;
        }
        
        var defenderPos = defender.transform.position;
        defenderPos.y = 0.5f;

        if (!attacking)
        {
            _animator.SetBool("RUN", false);
            _animator.SetBool("Attack", true);
            attacking = true;
        }
        
        var distance = Vector3.Distance(transform.position, defenderPos);
        //Check if player is in range to attack
        if (distance > 1)
        {
            transform.position = Vector3.Lerp(transform.position, defenderPos, Time.deltaTime * speed * 1.1f);
            var direction = defenderPos - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        
        //Attack timer
        timer -= Time.deltaTime;
        Vector3 offset = new Vector3(0, 0.5f, 0);

        if (timer <= 0)
        {
            if(defender == null) return;

            var damage = defender.GetComponent<DDefence>().GetParentObject();
           
            if(distance <= 2) damage.Damage(attackDamage);

            timer = attackTimer;
        }


    }

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Defender"))
        {
            //Add defender to queue on Trigger
            defenders.Enqueue(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Defender"))
        {
            if (defenders.Contains(other.gameObject))
            {
                //Remove defender to queue on TriggerExit
                defenders.Dequeue();
            }

            if (defenders.Count == 0)
            {
                attacking = false;
            }
        }
    }
}


//Setup for state machine
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

    //Attack Constructor
    public AttackState(Animator anim, GameObject target, float attackRate)
    {
        this.anim = anim;
        this.target = target;
        this.attackRate = attackRate;

        timer = attackRate;
    }

    public void OnEnter()
    {
        //Set Anim controller
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
