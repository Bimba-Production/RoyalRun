using System;
using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerStates
{
    public sealed class Fall : State, IEnterState
    {
        public StateNames Name { get; set;} = StateNames.Fall;
        public Fall(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous) => _animator.SetTrigger(PlayerAnimationTriggers.Fall.ToString());
    }
}
