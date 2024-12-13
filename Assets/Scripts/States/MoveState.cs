using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace States.Enemy
{
    public class MoveState : IState
    {
        private readonly Transform _transform;
        private readonly Vector3 _targetPosition;
        private readonly float _speed;

        private CancellationTokenSource _cancellationTokenSource;

        public event Action Complete;

        public MoveState(Transform transform, Vector3 targetPosition, float speed)
        {
            _transform = transform;
            _targetPosition = targetPosition;
            _speed = speed;
        }

        public void Enter()
        {
            _cancellationTokenSource ??= new CancellationTokenSource();

            Move().Forget();
        }

        private async UniTaskVoid Move()
        {
            while (Vector2.Distance(_transform.position, _targetPosition) > 0.1f)
            {
                _transform.position = Vector3.MoveTowards(
                    _transform.position,
                    _targetPosition,
                    _speed * Time.deltaTime);

                await UniTask.Yield(cancellationToken: _cancellationTokenSource.Token)
                    .SuppressCancellationThrow();
            }
        }

        public void Exit()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
            
            Complete?.Invoke();
        }
    }
}