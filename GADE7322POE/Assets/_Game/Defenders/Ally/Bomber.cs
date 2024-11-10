using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public float attackDistance = 1f;
    public ParticleSystem bombEffect;
    
    public Animator animator;
    
    private static string run = "Run";
    private int _runHashCode = Animator.StringToHash(run);
    
    
    private bool _isAttacking;

    private void OnEnable()
    {
        _isAttacking = false;
        animator = GetComponent<Animator>();
        DefendersController.AddDefender(gameObject);
    }

    private void Update()
    {
        var helper = EnemyAmountHandler.GetClosestEnemy(transform);
        var distance = Vector3.Distance(transform.position, helper.transform.position);
        var direction = helper.transform.position - transform.position;

        transform.forward = direction;
        
        
        if (distance < attackDistance)
        {
            if (_isAttacking) return;
            
            Debug.Log("Attacking");
            bombEffect.Play();
            _isAttacking = true;
        }
        else
        {
            if (_isAttacking)
            {
                EnemyAmountHandler.ChangeAnim(animator, _runHashCode);
                _isAttacking = false;
            }
            EnemyAmountHandler.ChaseEnemy(transform, 1f);
        }
    }

    public void Attack()
    {
        var helper = EnemyAmountHandler.GetClosestEnemy(transform);
        
    }
}
