using System;
using _Scripts.Audio;
using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerStates
{
    public sealed class Jump : State, IEnterState, IUpdateState, IExitState
    {
        public StateNames Name { get; set;} = StateNames.Jump;

        private readonly float _jumpForce = 12f;
        private readonly float _timer = 0.6f;
        private float _completeCooldownTime = 0f;
        private bool _timerActive = false;

        public Jump(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            AudioEffectController.Instance.Play(AudioEffectNames.jump, PlayerMover.Instance.transform.position);
            
            _controller.ResetAllTriggers();
            
            _mover.IsGrounded = false;
            _controller.CanLand = false;
            _controller.IsJumping = true;
            _completeCooldownTime = Time.realtimeSinceStartup + _timer;
            _timerActive = true;
            
            _mover.ApplyForce(Vector3.up, _jumpForce);
            
            if (previous.Name == StateNames.Run
                || previous.Name == StateNames.Sliding
                || previous.Name == StateNames.Roll
                || previous.Name == StateNames.Stumble) _animator.SetTrigger(PlayerAnimationTriggers.Jump.ToString());
        }

        public void Update()
        {
            _mover.Move();
            
            if (_timerActive && Time.realtimeSinceStartup >= _completeCooldownTime)
            {
                _timerActive = false;
                _controller.CanLand = true;
            }
            else _mover.UpdateIsGrounded();
        }

        public void Exit()
        {
            _controller.IsJumping = false;
            _mover.IsGrounded = true;
        }
    }
}
