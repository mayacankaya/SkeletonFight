using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AlictusGD
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private VariableJoystick _joystick;
        [SerializeField] CharacterController _controller;
        [SerializeField] Animator _animator;
        [SerializeField] public Image _healthBar;
        [SerializeField] private float _movementSpeed, _rotationSpeed, _currentHealth, _maxHealth=100;
        private void Awake()
        {
            _currentHealth = _maxHealth;
        }
        private void FixedUpdate()
        {
            var movementDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
            _controller.SimpleMove(movementDirection * _movementSpeed);

            if (movementDirection.sqrMagnitude <= 0)
            {
                _animator.SetBool("Run", false);
                return;
            }
            _animator.SetBool("Run", true);

            var targetDirection = Vector3.RotateTowards(_controller.transform.forward, movementDirection, _rotationSpeed * Time.deltaTime, 0);
            _controller.transform.rotation = Quaternion.LookRotation(targetDirection);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "EnemyProjectile")
            {
                Debug.Log(collision.gameObject.tag);
                DamageReceived(10);
            }
            else if (collision.gameObject.tag == "Enemy")
            {
                DamageReceived(_currentHealth);
            }
        }
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
                this.GetComponent<CapsuleCollider>().enabled = false;
                _animator.SetTrigger("Death");
                Invoke(nameof(GameManager.Instance.GameOver), 2);
            }
        }
    }
}