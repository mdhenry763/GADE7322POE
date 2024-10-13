using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GeneralEnemyController : MonoBehaviour
{
    [Header("Attack Settings: ")]
    public float attackTimer = 3f;
    public float attackDistance = 2f;
    public float aggroDistance = 5f;
    public int attackDamage = 10;
    public float speed = 1;

    [Header("Animation Settings")] 
    public Animator animator;

    public string walkAnim;
    public string attackAnim;

    private int walkAnimHash = 0;
    private int attackAnimHash = 0;

    private Queue<Vector3> pathQueue;
    
    private Vector3 currentPos;
    private Vector3 nextPos;

    private bool spawned;

    private bool _isAttacking;
    private float timer;

    private void OnEnable()
    {
        //Setup variables
        pathQueue = new Queue<Vector3>();
        spawned = false;
        timer = attackTimer;

        walkAnimHash = Animator.StringToHash(walkAnim);
        attackAnimHash = Animator.StringToHash(attackAnim);
    }

    public void InitEnemy(List<Vector3> path, Vector3 offset, float speed) //Initialise the enemy
    {
        foreach (var point in path)
        {
            pathQueue.Enqueue(point);
        }
        
        this.speed = speed;

        //Setup starting pos and next pos
        transform.position = path[0];
        nextPos = path[0];
        nextPos.y = 0.5f;
        spawned = true;
    }

    private void Update()
    {
        if (!spawned) return;

        var defender = DefendersController.GetClosestDefender(transform);
        if (defender != null)
        {
            var distance = Vector3.Distance(transform.position, defender.transform.position);
            if (distance <= aggroDistance)
            {
                AttackDefender(defender);
            }
            else
            {
                FollowPath();
            }

            return;
        }

        speed = 1;
        FollowPath();
    }

    private void FollowPath()
    {
        if (pathQueue.Count == 0) return;
        if (_isAttacking)
        {
            animator.CrossFade(walkAnimHash, 0.1f);
            _isAttacking = false;
        }

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

    private void AttackDefender(GameObject defender) //Attack defender obj
    {
        var defenderPos = defender.transform.position;
        
        var distance = Vector3.Distance(transform.position, defenderPos);
        //Check if player is in range to attack
        if (distance > attackDistance)
        {
            transform.position = Vector3.Lerp(transform.position, defenderPos, Time.deltaTime * speed * 1.1f);
            var direction = defenderPos - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            if (!_isAttacking)
            {
                animator.CrossFade(attackAnimHash, 0.1f);
                _isAttacking = true;
            }
            
            timer -= Time.deltaTime;

            if (!(timer <= 0)) return;
            if(defender == null) return;

            var damage = defender.GetComponent<DDefence>().GetParentObject();
           
            if(distance <= attackDistance) damage.Damage(attackDamage);

            timer = attackTimer;
        }
    }

    public void DealAttackDamage()
    {
        var defender = DefendersController.GetClosestDefender(transform);
        if (defender == null) return;
        
        var damage = defender.GetComponent<DDefence>().GetParentObject();
        damage.Damage(attackDamage);

    }
}