using System;

namespace States
{
    public interface IState
    {
        public event Action Complete;
        void Enter();
        void Exit();
    }
}