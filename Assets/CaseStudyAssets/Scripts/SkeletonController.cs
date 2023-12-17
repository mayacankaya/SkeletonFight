using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace AlictusGD
{
    public class SkeletonController : MonoBehaviour
    {
        public NavMeshAgent agent;
        [SerializeField] GameObject _hand;
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
        public bool playerInAttackRange, isDeath = false;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
        }
        bool animB = true;

        private void Update()
        {
            if (!isDeath)
            {
                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
                if (!playerInAttackRange) ChasePlayer();
                else if (playerInAttackRange) { anim(); Invoke(nameof(AttackPlayer), 1.8f); }
            }
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
        //takip etme
        private void ChasePlayer()
        {

            agent.SetDestination(player.position);
            _animator.SetTrigger("Run");

        }
        GameObject Go;
        //Ateþ etme Kodu
        private void AttackPlayer()
        {
            if (agent != null)
            {
                //yürümesini engeller
                agent.SetDestination(transform.position);
                transform.LookAt(player);
                if (!alreadyAttacked)
                {
                    ///Attack code here
                    Go = Instantiate(projectile, transform.position, Quaternion.identity);
                    Rigidbody rb = Go.GetComponent<Rigidbody>();
                    rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                    rb.AddForce(transform.up * 8f, ForceMode.Impulse);
                    ///End of attack code
                    alreadyAttacked = true;
                    animB = true;
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }
            }
        }
        private void ResetAttack()
        {
            alreadyAttacked = false;
            Destroy(Go);
        }
        //Ölme Kodu
        public void DestroyEnemy()
        {
            isDeath = true;
            alreadyAttacked = true;
            GameManager.Instance._killedEnemy++;
            GameManager.Instance._enemyText.text = GameManager.Instance._killedEnemy.ToString();
            Destroy(Go);
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            PlayerController.Instance.As.clip = PlayerController.Instance._attackCl;
            PlayerController.Instance.As.Play();
            _animator.SetTrigger("Death");
            StartCoroutine(animFinish());
        }
        IEnumerator animFinish()
        {
            yield return new WaitForSeconds(2.5f);
            GameManager.Instance.EnemyCreate(1);
            Destroy(gameObject);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}