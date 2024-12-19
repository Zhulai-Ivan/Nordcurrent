using System;
using Enemies;
using Modules;
using Player.Input;
using UnityEngine;
using View;
using Zenject;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] private PlayerView _view;

        private InputManager _inputManager;
        private IViewPool _viewPool;

        public event Action Dead;

        [Inject]
        private void InstallBindings(InputManager inputManager, IViewPool viewPool)
        {
            _inputManager = inputManager;
            _viewPool = viewPool;
            
            _view.Setup(this);
        }

        private void OnEnable()
        {
            Subscribe();
            
            _view.SetTrack(new FastTrack());
        }

        private void Subscribe()
        {
            _inputManager.Move += OnMove;
            _inputManager.Rotate += OnRotate;
            _inputManager.Fire += OnFire;
        }

        private void OnRotate(float direction)
        {
            float rotationChange = -direction * _view.Speed * 10f * Time.fixedDeltaTime;
            float targetAngle = transform.eulerAngles.z + rotationChange;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Mathf.Abs(rotationChange));
        }

        private async void OnFire()
        {
            var bullet = await _viewPool.Pop<Bullet.Bullet>(_bulletSpawnPoint.position, Vector3.one, Vector3.one, null);
            bullet.Direction = _bulletSpawnPoint.up.normalized;
        }

        private void OnMove(float direction)
        {
            Vector2 moveDirection = transform.up * direction;
            _rigidbody.MovePosition(_rigidbody.position + moveDirection * _view.Speed * Time.fixedDeltaTime);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<Enemy>(out var _))
            {
               Dead?.Invoke();
               _view.Push();
            }
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            _inputManager.Move -= OnMove;
            _inputManager.Rotate -= OnRotate;
            _inputManager.Fire -= OnFire;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }
    }
}