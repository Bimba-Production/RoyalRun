using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerStates
{
    public sealed class Sliding : State, IEnterState, IUpdateState, IExitState
    {
        private readonly float _timer = 0.8f;
        private float _currentTimer = 0.8f;
        private bool _timerActive = false;

        public Sliding(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _controller.ColliderAnimator.SetTrigger(ColliderAnimationTrigger.Sit.ToString());
            if (previous is Run) _animator.SetTrigger(PlayerAnimationTriggers.RunToSliding.ToString());

            _controller.CanLand = false;
            _controller.IsSliding = true;
            _timerActive = true;
        }

        public void Update()
        {
            _mover.Move();

            if (_timerActive)
            {
                if (_currentTimer <= 0)
                {
                    _timerActive = false;
                    _controller.IsSliding = false;
                    _controller.CanLand = true;
                    _currentTimer = _timer;
                }
                else _currentTimer -= Time.deltaTime;
            }
        }

        public void Exit() {
            _controller.IsSliding = false;
            _controller.ColliderAnimator.SetTrigger(ColliderAnimationTrigger.Stand.ToString());
        }
    }
}
