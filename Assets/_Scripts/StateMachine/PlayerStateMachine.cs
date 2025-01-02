using System.Collections.Generic;
using _Scripts.StateMachine.Interfaces;

namespace _Scripts.StateMachine
{
    public class PlayerStateMachine
    {
        private IState _current;
        private IState _previous;
        private readonly List<IState> _states;
        private readonly Dictionary<IState, List<Transition>> _transitionsPerState;
        private bool _canTransition = true;

        public PlayerStateMachine(List<IState> states, Transition[] transitions, IState startState)
        {
            _states = states;
            _current = startState;
            _transitionsPerState = new Dictionary<IState, List<Transition>>();

            foreach (Transition transition in transitions)
            {
                if (!_transitionsPerState.ContainsKey(transition.From)) _transitionsPerState[transition.From] = new List<Transition>() { transition };
                else _transitionsPerState[transition.From].Add(transition);
            }
        }

        public void Update()
        {
            if (_current is IUpdateState updateStates) updateStates.Update();

            if (_canTransition)
            {
                foreach (Transition transition in _transitionsPerState[_current])
                {
                    if (transition.Condition())
                    {
                        _canTransition = false;
                        TransitionTo(transition.To);
                        return;
                    }
                }
            }
        }

        public void FixedUpdate()
        {
            if (_current is IFixedUpdateState fixedUpdateState) fixedUpdateState.FixedUpdate();
        }

        private void TransitionTo(IState target)
        {
            _previous = _current;
            if (_current is IExitState exitState) exitState.Exit();
            if (_states.Contains(target)) _current = target;
            if (_current is IEnterState enterState) enterState.Enter(_previous);
            _canTransition = true;
        }
    }
}
