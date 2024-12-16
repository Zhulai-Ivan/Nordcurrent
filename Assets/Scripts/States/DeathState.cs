using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace States
{
    public class DeathState : IState
    {
        private CancellationTokenSource _cancellationTokenSource;
        
        public event Action Complete;
        public void Enter()
        {
            _cancellationTokenSource ??= new CancellationTokenSource();

            Dead().Forget();
        }

        private async UniTaskVoid Dead()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}