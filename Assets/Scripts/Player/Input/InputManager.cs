using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Input
{
    public class InputManager : MonoBehaviour
    {
        private float _moveDirection = 0f;

        public event Action<float> Move;
        public event Action<float> Rotate;
        public event Action Fire;

        private PlayerControls _playerControls;
        private InputAction _moveInput;
        private InputAction _rotateInput;
        private InputAction _fireInput;

        private void Awake()
        {
            _playerControls = new PlayerControls();

            _moveInput = _playerControls.Player.Walk;
            _rotateInput = _playerControls.Player.Rotate;
            _fireInput = _playerControls.Player.Fire;
            _fireInput.performed += CallFireAction;
            
            _playerControls.Enable();
        }

        private void CallFireAction(InputAction.CallbackContext obj) =>
            Fire?.Invoke();

        private void FixedUpdate()
        {
            Move?.Invoke(_moveInput.ReadValue<float>());
            Rotate?.Invoke(_rotateInput.ReadValue<float>());
        }

        private void OnDestroy()
        {
            _fireInput.performed -= CallFireAction;
        }
    }
}