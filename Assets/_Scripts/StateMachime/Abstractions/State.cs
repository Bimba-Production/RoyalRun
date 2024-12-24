using UnityEngine;

namespace Assets._Scripts.StateMachime
{
    public abstract class State
    {
        protected Animator _animator;
        protected PlayerMover _mover;
        protected PlayerController _controller;

        public State(Animator animator, PlayerMover mover, PlayerController controller)
        {
            _animator = animator;
            _mover = mover;
            _controller = controller;
        }
    }
}
