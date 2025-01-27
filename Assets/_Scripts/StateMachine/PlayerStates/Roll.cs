using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerStates
{
    public sealed class Roll : State, IEnterState, IUpdateState, IExitState
    {
        private readonly float _speed = 8f;
        private readonly float _force = 11f;
        private readonly float _timer = 0.3f;
        private float _currentTimer = 0.3f;
        private bool _timerActive = false;

        public Roll(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _mover.ApplyForce(Vector3.down, _force);

            _controller.ColliderAnimator.SetTrigger(ColliderAnimationTrigger.Sit.ToString());
            if (previous is Jump) _animator.SetTrigger(PlayerAnimationTriggers.Roll.ToString());

            _controller.IsJumping = false;
            _controller.IsRolling = true;
            _timerActive = true;
        }

        public void Update()
        {
            _mover.Move(_controller.MinX, _controller.MaxX, _speed);

            if (_timerActive)
            {
                if (_currentTimer <= 0)
                {
                    _timerActive = false;
                    _controller.CanLand = true;
                    _controller.IsRolling = false;
                    _currentTimer = _timer;
                }
                else _currentTimer -= Time.deltaTime;
            }
            else _mover.UpdateIsGrounded();
        }

        public void Exit()
        {
            _controller.IsRolling = false;
            _controller.ColliderAnimator.SetTrigger(ColliderAnimationTrigger.Stand.ToString());
        }
    }
}
