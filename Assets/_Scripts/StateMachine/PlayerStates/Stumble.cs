using System;
using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerStates
{
    public sealed class Stumble : State, IEnterState, IUpdateState, IExitState
    {
        public StateNames Name { get; set;} = StateNames.Stumble;
        
        public Stumble(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _controller.IsStumble = false;
            _controller.CurrentCriticalCooldown = _controller.CriticalCooldown;
            _controller.IsCriticalCondition = true;
            _controller.CameraController.ApplyDamageEffect();
            
            _controller.CanLand = false;

            if (previous.Name == StateNames.Run) _animator.SetTrigger(PlayerAnimationTriggers.Stumble.ToString());
        }

        public void Update()
        {
            _mover.Move();
            _mover.UpdateIsGrounded();
            
            _controller.CanLand = true;
        }

        public void Exit() => _animator.ResetTrigger(PlayerAnimationTriggers.Stumble.ToString());
    }
}
