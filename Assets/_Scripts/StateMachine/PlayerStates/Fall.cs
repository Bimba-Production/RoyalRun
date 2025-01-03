﻿using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerStates
{
    public class Fall : State, IEnterState
    {
        public Fall(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous) => _animator.SetTrigger(PlayerAnimationTriggers.Fall.ToString());
    }
}
