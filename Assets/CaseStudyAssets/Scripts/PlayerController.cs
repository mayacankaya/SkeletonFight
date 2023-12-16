using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private VariableJoystick _joystick;
    [SerializeField] CharacterController _controller;
    [SerializeField] Animator _animator;
    [SerializeField] private float _movementSpeed, _rotationSpeed;

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

        var targetDirection = Vector3.RotateTowards(_controller.transform.forward, movementDirection, _rotationSpeed * Time.deltaTime,0);
        _controller.transform.rotation = Quaternion.LookRotation(targetDirection);
    }

}
