using System;
using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerStates
{
    public sealed class Sliding : State, IEnterState, IUpdateState, IExitState
    {
        public String Name { get; set;} = "Sliding";

        private readonly float _timer = 0.8f;
        private float _currentTimer = 0.8f;
        private bool _timerActive = false;

        public Sliding(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _currentTimer = _timer;
            _controller.ResetAllTriggers();
            _controller.ColliderAnimator.SetTrigger(ColliderAnimationTrigger.Sit.ToString());
            if (previous is Run) _animator.SetTrigger(PlayerAnimationTriggers.RunToSliding.ToString());

            _controller.CanLand = false;
            _controller.IsSliding = true;
            _timerActive = true;
        }

        public void Update()
        {
            _mover.Move();
            // Debug.Log(_controller.CanLand.ToString());
            //
            // Debug.Log(_timerActive.ToString());
            if (_timerActive)
            {
                // Debug.Log(_currentTimer.ToString());

                if (_currentTimer <= 0)
                {
                    _timerActive = false;
                    _controller.IsSliding = false;
                    _controller.CanLand = true;
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
