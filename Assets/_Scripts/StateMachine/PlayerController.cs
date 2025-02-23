﻿using System;
using System.Collections.Generic;
using _Scripts.Camera;
using _Scripts.StateMachine.Interfaces;
using _Scripts.StateMachine.PlayerStates;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.StateMachine
{
    public sealed class PlayerController: Singleton<PlayerController>
    {
        [Header("References")]
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private Animator _animator;
        [SerializeField] private Animator _colliderAnimator;
        [SerializeField] private PlayerMover _mover;
        [SerializeField] private String _currentState;

        
        public bool IsCriticalCondition = false;
        public bool IsJumping = false;
        public bool CanLand = false;
        public bool IsSliding = false;
        public bool IsRolling = false;
        public bool IsStumble = false;
        public bool IsFall = false;
        public bool Restart = false;
        public float CurrentCriticalCooldown = 0f;

        public UnityEvent OnGameOverEvent;

        private PlayerStateMachine _stateMachine;

        private void Start() => InitStateMachine();
        
        public Animator PlayerAnimator() => _animator;
        
        public Animator ColliderAnimator => _colliderAnimator;
        public float CriticalCooldown { get; } = 3;

        public CameraController CameraController => _cameraController;

        private void Update()
        {
            _stateMachine.Update();
            _currentState = _stateMachine.CurrentState;
            
            if (!IsCriticalCondition) return;
            
            if (CurrentCriticalCooldown > 0) CurrentCriticalCooldown -= Time.deltaTime;
            else
            {
                IsCriticalCondition = false;
                _cameraController.DisableDamageEffect();
                GameController.Instance.DeactivateRock();
                CameraController.Instance.ResetPosition();
            }
        }

        private void FixedUpdate() => _stateMachine.FixedUpdate();

        private void InitStateMachine()
        {
            Run run = new Run(_animator, _mover, this);
            Jump jump = new Jump(_animator, _mover, this);
            Stumble stumble = new Stumble(_animator, _mover, this);
            Sliding sliding = new Sliding(_animator, _mover, this);
            Fall fall = new Fall(_animator, _mover, this);
            Roll roll = new Roll(_animator, _mover, this);

            List<IState> states = new List<IState> {
                run,
                jump,
                stumble,
                sliding,
                fall,
                roll
            };

            Transition[] transitions = {
                //Run
                new Transition(run, fall, CanFall),
                new Transition(run, stumble, CanStumble),
                new Transition(run, sliding, CanSliding),
                new Transition(run, jump, CanJump),

                //Sliding
                new Transition(sliding, fall, CanFall),
                new Transition(sliding, run, CanRun),
                new Transition(sliding, jump, CanJump),

                //Jump
                new Transition(jump, fall, CanFall),
                new Transition(jump, run, CanRun),
                new Transition(jump, roll, CanRoll),

                //Roll
                new Transition(roll, fall, CanFall),
                new Transition(roll, run, CanRun),
                new Transition(roll, jump, CanJump),

                //Stumble
                new Transition(stumble, fall, CanFall),
                new Transition(stumble, run, CanRun),
                new Transition(stumble, jump, CanJump),

                //Fall
                new Transition(fall, run, CanRestart),
            };

            _stateMachine = new PlayerStateMachine(states, transitions, run);
        }

        private bool CanRun() => _mover.IsGrounded && !IsSliding && !IsRolling && CanLand;
        private bool CanSliding() => _mover.IsGrounded && !IsSliding && !IsRolling && !IsJumping && _mover.SlidingInput;
        private bool CanJump() => !IsJumping && !IsRolling && _mover.JumpInput && _mover.IsGrounded;
        private bool CanRoll() => IsJumping && _mover.SlidingInput && !IsRolling && !_mover.IsGrounded;
        private bool CanStumble() => _mover.IsGrounded && IsStumble;
        private bool CanFall() => IsFall;
        private bool CanRestart() => Restart && !IsFall;

        public void Reset()
        {
            ResetAllTriggers();

            IsFall = false;
            Restart = true;
            _animator.SetTrigger(PlayerAnimationTriggers.Reset.ToString());
        }

        public void ResetAllTriggers()
        {
            _animator.ResetTrigger(PlayerAnimationTriggers.Fall.ToString());
            _animator.ResetTrigger(PlayerAnimationTriggers.Jump.ToString());
            _animator.ResetTrigger(PlayerAnimationTriggers.Roll.ToString());
            _animator.ResetTrigger(PlayerAnimationTriggers.Land.ToString());
            _animator.ResetTrigger(PlayerAnimationTriggers.Stumble.ToString());
            _animator.ResetTrigger(PlayerAnimationTriggers.RunToSliding.ToString());
        }
    }

    public enum StateNames
    {
        Run = 1,
        Jump = 2,
        Fall = 3,
        Roll = 4,
        Sliding = 5,
        Stumble = 6,
    }
}
