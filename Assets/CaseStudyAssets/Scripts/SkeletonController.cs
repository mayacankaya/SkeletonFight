using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : MonoBehaviour
{
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Transform _player;
    [SerializeField] LayerMask _isGround,_isPlayer;
    [SerializeField] Vector3 _walkPoint;
    bool _walkPointSet,alreadyAttacked;
    [SerializeField] GameObject projectile;
    [SerializeField] float _walkPointRange,_timeBetweenAttacks,_attackRange,_health;
    public bool  _playerInAttackRange;

    private void Update()
    {
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _isPlayer);
        if (!_playerInAttackRange)
        {
            ChasePlayer();
        }
        else
        {
            AttackPlayer();
        }
    }

    private void ChasePlayer()
    {
        _agent.SetDestination(_player.position);
    }
    private void AttackPlayer()
    {
        // attack 

        Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 32, ForceMode.Impulse);
        rb.AddForce(transform.up * 8, ForceMode.Impulse);



        _agent.SetDestination(transform.position);
        transform.LookAt(_player);
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 2);
        }
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
       
    }
}
