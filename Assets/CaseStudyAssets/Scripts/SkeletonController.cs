using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace AlictusGD
{
    public class SkeletonController : MonoBehaviour
    {
        public NavMeshAgent agent;

        public Transform player;
        [SerializeField] Animator _animator;
        public LayerMask whatIsGround, whatIsPlayer;

        public float health;

        //Patroling
        public Vector3 walkPoint;
        bool walkPointSet;
        public float walkPointRange;

        //Attacking
        public float timeBetweenAttacks;
        bool alreadyAttacked;
        public GameObject projectile;

        //States
        public float attackRange;
        public bool playerInAttackRange;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
        }
        bool animB = true;
        private void Update()
        {
            //Check for sight and attack range

            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


            if (!playerInAttackRange) ChasePlayer();
            else if (playerInAttackRange) { anim(); Invoke(nameof(AttackPlayer), 1.8f); }
        }

        void anim()
        {
            if (animB)
            {
                _animator.SetTrigger("Throw");
                agent.SetDestination(transform.position);
                animB = false;
            }
        }
        private void ChasePlayer()
        {

            agent.SetDestination(player.position);
            _animator.SetTrigger("Run");

        }

        private void AttackPlayer()
        {
            //Make sure enemy doesn't move
            agent.SetDestination(transform.position);

            transform.LookAt(player);

            if (!alreadyAttacked)
            {
                ///Attack code here
                Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                rb.AddForce(transform.up * 8f, ForceMode.Impulse);
                ///End of attack code

                alreadyAttacked = true;
                animB = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
        private void ResetAttack()
        {
            alreadyAttacked = false;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;

            if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
        }
        private void DestroyEnemy()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}