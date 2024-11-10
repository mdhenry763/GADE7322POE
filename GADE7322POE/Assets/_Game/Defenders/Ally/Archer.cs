using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Archer : MonoBehaviour
{
    public float attackDistance = 5f;
    public GameObject arrow;
    public Animator animator;
    
    private static string attack = "Attack 1";
    private static string run = "Run";

    private int _attackHashCode = Animator.StringToHash(attack);
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
            animator.CrossFade(_attackHashCode, 0.1f);
            EnemyAmountHandler.ChangeAnim(animator, _attackHashCode);
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
        var spawnedArrow = Instantiate(arrow, transform.position + new Vector3(0, 0.25f, 0), quaternion.identity);
        var helper = EnemyAmountHandler.GetClosestEnemy(transform);
        var direction = helper.transform.position - transform.position;
        var arrowLogic =  spawnedArrow.GetComponent<CannonBallLogic>();
        arrowLogic.damageAmount = 10;
        arrowLogic.InitMotion(direction);
    }
}
