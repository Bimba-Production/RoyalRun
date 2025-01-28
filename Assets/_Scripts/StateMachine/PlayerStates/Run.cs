using System;
using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerStates
{
    public sealed class Run : State, IEnterState, IUpdateState
    {
        public String Name { get; set;} = "Run";
        
        public Run(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _controller.ResetAllTriggers();
            _controller.Restart = false;
            
            Debug.Log((previous is Jump).ToString());
            Debug.Log((previous is Sliding).ToString());
            Debug.Log(previous.Name);
            Debug.Log("_________________");


            if (previous is Jump)_animator.SetTrigger(PlayerAnimationTriggers.Land.ToString());
            else if (previous is Sliding) _animator.SetTrigger(PlayerAnimationTriggers.SlidingToRun.ToString());
        }

        public void Update()
        {
            _mover.Move();
            _mover.UpdateIsGrounded();
        }
    }
}
