namespace States
{
    public class StateMachine
    {
        private IState _currentState;

        public void SetState(IState state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState?.Enter();
        }
    }
}