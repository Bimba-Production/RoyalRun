using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerStates
{
    public sealed class Run : State, IEnterState, IUpdateState
    {
        private readonly float _speed = 8f;

        public Run(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _controller.Restart = false;
                        
            if (previous is Jump)_animator.SetTrigger(PlayerAnimationTriggers.Land.ToString());
            else if (previous is Sliding) _animator.SetTrigger(PlayerAnimationTriggers.SlidingToRun.ToString());
        }

        public void Update()
        {
            _mover.Move(_controller.MinX, _controller.MaxX, _speed);
            _mover.UpdateIsGrounded();
        }
    }
}
