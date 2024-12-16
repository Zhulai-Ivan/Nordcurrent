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

        private InputManager _inputManager;
        private IViewPool _viewPool;

        [Inject]
        private void InstallBindings(InputManager inputManager, IViewPool viewPool)
        {
            _inputManager = inputManager;
            _viewPool = viewPool;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            _inputManager.Move += OnMove;
            _inputManager.Rotate += OnRotate;
            _inputManager.Fire += OnFire;
        }

        private void OnRotate(float direction)
        {
            transform.Rotate(Vector3.back, direction * Time.fixedDeltaTime);
        }

        private async void OnFire()
        {
            var bullet = await _viewPool.Pop<Bullet.Bullet>(_bulletSpawnPoint.position, null);
            bullet.Direction = _bulletSpawnPoint.up.normalized;
        }

        private void OnMove(float direction)
        {
            Vector2 moveDirection = transform.up * direction;
            _rigidbody.MovePosition(_rigidbody.position + moveDirection * Time.fixedDeltaTime);
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