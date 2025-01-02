using UnityEngine;

namespace _Scripts.StateMachine.Abstractions
{
    public abstract class State
    {
        protected Animator _animator;
        protected PlayerMover _mover;
        protected PlayerController _controller;

        protected State(Animator animator, PlayerMover mover, PlayerController controller)
        {
            _animator = animator;
            _mover = mover;
            _controller = controller;
        }
    }
}
