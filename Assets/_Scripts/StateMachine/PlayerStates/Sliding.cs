using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;
using System;

namespace _Scripts.StateMachine.PlayerStates
{
    public sealed class Sliding : State, IEnterState, IUpdateState, IExitState
    {
        public String Name { get; set;} = nameof(StateNames.Sliding);

        private readonly float _timer = 0.8125f;
        private float _completeCooldownTime = 0f;
        private bool _timerActive = false;

        public Sliding(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _completeCooldownTime = Time.realtimeSinceStartup + _timer;
            _controller.ResetAllTriggers();
            _controller.ColliderAnimator.SetTrigger(ColliderAnimationTrigger.Sit.ToString());
            if (previous.Name == nameof(StateNames.Run)) _animator.SetTrigger(PlayerAnimationTriggers.RunToSliding.ToString());

            _controller.CanLand = false;
            _controller.IsSliding = true;
            _timerActive = true;
        }

        public void Update()
        {
            _mover.Move();
            
            if (_timerActive && Time.realtimeSinceStartup >= _completeCooldownTime)
            {
                _completeCooldownTime = 0f;
                _timerActive = false;
                _controller.IsSliding = false;
                _controller.CanLand = true;
            }
        }

        public void Exit() {
            _controller.CanLand = false;
            _controller.IsSliding = false;
            _controller.ColliderAnimator.SetTrigger(ColliderAnimationTrigger.Stand.ToString());
        }
    }
}
