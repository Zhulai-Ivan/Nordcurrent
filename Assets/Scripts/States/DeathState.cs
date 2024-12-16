using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using View;

namespace States
{
    public class DeathState : IState
    {
        private CancellationTokenSource _cancellationTokenSource;
        
        private readonly BaseView _view;

        public event Action Complete;

        public DeathState(BaseView view)
        {
            _view = view;
        }
        public void Enter()
        {
            _cancellationTokenSource ??= new CancellationTokenSource();
            _view.Push();
            
            Dead().Forget();
        }

        private async UniTaskVoid Dead()
        {
            await UniTask.Delay(1000, cancellationToken: _cancellationTokenSource.Token)
                .SuppressCancellationThrow();

            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
                Complete?.Invoke();
            
        }

        public void Exit()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
        }
    }
}