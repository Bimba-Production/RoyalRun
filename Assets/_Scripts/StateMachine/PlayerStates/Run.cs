using System;
using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerStates
{
    public sealed class Run : State, IEnterState, IUpdateState, IExitState
    {
        public StateNames Name { get; set;} = StateNames.Run;
        private IState _previous;
        
        public Run(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _controller.ResetAllTriggers();
            _controller.Restart = false;
            
            if (previous.Name == StateNames.Jump)_animator.SetTrigger(PlayerAnimationTriggers.Land.ToString());
        }

        public void Update()
        {
            _mover.Move();
            _mover.UpdateIsGrounded();
        }
        
        public void Exit() => _controller.ResetAllTriggers();
    }
}
