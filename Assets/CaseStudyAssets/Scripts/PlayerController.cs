using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AlictusGD
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;
        [SerializeField] private VariableJoystick _joystick;
        [SerializeField] CharacterController _controller;
        [SerializeField] Animator _animator;
        [SerializeField] public Image _healthBar;
        [SerializeField] GameObject _hand;
        [SerializeField] GameObject _boomerangGo, boomerangG;
        [SerializeField] public AudioSource As;
        [SerializeField] public AudioClip _coinCl, _enemyCl, _projectileCl, _attackCl;
        [SerializeField] float timeBetweenAttacks;
        //[SerializeField] public List<GameObject> AttacksList = new List<GameObject>();
        [SerializeField] private float _movementSpeed, _rotationSpeed, _currentHealth, _maxHealth = 100;
        bool alreadyAttacked, gameOverB = false;
        int _collectedCoin;
        private void Awake()
        {
            Instance = this;
            _currentHealth = _maxHealth;
        }
        //Joystick hareket
        private void FixedUpdate()
        {
            if (!gameOverB)
            {
                var movementDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
                _controller.SimpleMove(movementDirection * _movementSpeed);

                if (movementDirection.sqrMagnitude <= 0)
                {
                    _animator.SetBool("Run", false);
                    return;
                }
               else _animator.SetBool("Run", true);

                var targetDirection = Vector3.RotateTowards(_controller.transform.forward, movementDirection, _rotationSpeed * Time.deltaTime, 0);
                _controller.transform.rotation = Quaternion.LookRotation(targetDirection);
            }
        }

        //Çarpýþma 
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.tag);
            if (collision.gameObject.tag == "EnemyProjectile")
            {
                As.clip = _projectileCl;
                As.Play();
                DamageReceived(10);
            }
            else if (collision.gameObject.tag == "Enemy")
            {
                As.clip = _enemyCl;
                As.Play();
                DamageReceived(_currentHealth);
            }
            else if (collision.gameObject.tag == "Coin")
            {
                As.clip = _coinCl;
                As.Play();
                GameManager.Instance._collectedCoin++;
                PlayerPrefs.SetInt("coin", GameManager.Instance._collectedCoin);
                GameManager.Instance._coinText.text = GameManager.Instance._collectedCoin.ToString();

                Destroy(collision.gameObject);
            }
        }

        //Atýþý baþlatma
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy")
            {
                if (!alreadyAttacked)
                {
                    alreadyAttacked = true;
                    Attack(other.transform.position);
                }
            }
        }

        //Ateþ et
        private void Attack(Vector3 enemy)
        {
            ///Attack code here
            boomerangG = Instantiate(_boomerangGo, transform.position, Quaternion.identity);
            boomerangG.GetComponent<BoomerangController>()._enemyPosition = enemy;
            //End of attack code
            ///
            Invoke(nameof(ResetAttack), timeBetweenAttacks); //Soðuma süresi belirtilir

        }
        private void ResetAttack()
        {
            alreadyAttacked = false;
            Destroy(boomerangG);
        }

        void Fire()
        {
            Debug.Log("attack");

        }
        //Hasar alma
        public void DamageReceived(float damage)
        {
            if (_currentHealth >= damage)
            {
                _currentHealth -= damage;
                _healthBar.fillAmount = _currentHealth / _maxHealth;
            }
            else
            {
                _currentHealth = 0;
                _healthBar.fillAmount = _currentHealth / _maxHealth;

            }
            if (_currentHealth <= 0)
            {
                _animator.SetBool("Run", false);
                gameOverB = true;
                this.GetComponent<CapsuleCollider>().enabled = false;
                _animator.SetTrigger("Death");
                Invoke(nameof(GameOver), 2);
            }
        }
        //Oyun sonu
        void GameOver()
        {
            GameManager.Instance.GameOver();

        }
    }
}