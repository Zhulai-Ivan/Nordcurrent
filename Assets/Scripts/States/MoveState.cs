using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace States
{
    public class MoveState : IState
    {
        private readonly Transform _transform;
        private readonly float _speed;
        private readonly Vector2 _xLimits;
        private readonly Vector2 _yLimits;

        private CancellationTokenSource _cancellationTokenSource;
        private Vector2 _targetPosition;
        public event Action Complete;

        public MoveState(Transform transform, Vector2 xLimits, Vector2 yLimits, float speed)
        {
            _transform = transform;
            _xLimits = xLimits;
            _yLimits = yLimits;
            _speed = speed;
        }

        public void Enter()
        {
            _cancellationTokenSource ??= new CancellationTokenSource();

            Move().Forget();
        }

        private async UniTaskVoid Move()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _targetPosition = new Vector2(
                    x: Random.Range(-_xLimits.x, _xLimits.x),
                    y: Random.Range(-_yLimits.y, _yLimits.y)
                );

                var direction = _targetPosition - (Vector2)_transform.position;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

                var tcs = new UniTaskCompletionSource();

                _transform.DORotate(new Vector3(0, 0, angle), _speed)
                    .OnComplete(() => tcs.TrySetResult());

                await tcs.Task;

                while (Vector2.Distance(_transform.position, _targetPosition) > 0.1f)
                {
                    _transform.position = Vector3.MoveTowards(
                        _transform.position,
                        _targetPosition,
                        _speed * Time.deltaTime
                    );

                    await UniTask.Yield(cancellationToken: _cancellationTokenSource.Token)
                        .SuppressCancellationThrow();
                }
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