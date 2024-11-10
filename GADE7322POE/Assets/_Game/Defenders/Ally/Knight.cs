using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public int damage = 4;
    public float attackDistance = 1f;
    public Animator animator;
    
    private static string attack = "Attack";
    private static string run = "Run 0";

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
        var helper = EnemyAmountHandler.GetClosestEnemy(transform);

        if (helper.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.Damage(damage);
        }

        var direction = helper.transform.position - transform.position;
    }
}
