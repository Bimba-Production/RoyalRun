using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerStates
{
    public sealed class Jump : State, IEnterState, IUpdateState, IExitState
    {
        private readonly float _jumpForce = 12f;
        private readonly float _timer = 0.6f;
        private float _currentTimer = 0.6f;
        private bool _timerActive = false;

        public Jump(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _mover.IsGrounded = false;
            _controller.CanLand = false;
            _controller.IsJumping = true;
            _timerActive = true;
            
            _mover.ApplyForce(Vector3.up, _jumpForce);
            
            if (previous is Run
                || previous is Sliding
                || previous is Roll
                || previous is Stumble) _animator.SetTrigger(PlayerAnimationTriggers.Jump.ToString());
        }

        public void Update()
        {
            _mover.Move();

            if (_timerActive)
            {
                if (_currentTimer <= 0)
                {
                    _timerActive = false;
                    _controller.CanLand = true;
                    _currentTimer = _timer;
                }
                else _currentTimer -= Time.deltaTime;
            }
            else _mover.UpdateIsGrounded();
        }

        public void Exit()
        {
            _controller.IsJumping = false;
            _controller.CanLand = false;
            _mover.IsGrounded = true;
        }
    }
}
