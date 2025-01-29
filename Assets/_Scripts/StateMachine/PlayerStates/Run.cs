using System;
using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerStates
{
    public sealed class Run : State, IEnterState, IUpdateState
    {
        public String Name { get; set;} = nameof(StateNames.Run);
        private IState _previous;
        
        public Run(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _controller.ResetAllTriggers();
            _controller.Restart = false;
            _previous = previous;
            Debug.Log(previous.Name);
            
            if (previous.Name == nameof(StateNames.Jump))_animator.SetTrigger(PlayerAnimationTriggers.Land.ToString());
            else if (previous.Name == nameof(StateNames.Sliding)) _animator.SetTrigger(PlayerAnimationTriggers.SlidingToRun.ToString());
        }

        public void Update()
        {
            if (_controller.CanLand)
            {
                
            }
            
            _mover.Move();
            _mover.UpdateIsGrounded();
        }
    }
}
