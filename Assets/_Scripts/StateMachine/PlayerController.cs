using System.Collections.Generic;
using _Scripts.Camera;
using _Scripts.StateMachine.Interfaces;
using _Scripts.StateMachine.PlayerStates;
using UnityEngine;

namespace _Scripts.StateMachine
{
    public class PlayerController: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private Animator _animator;
        [SerializeField] private Animator _colliderAnimator;
        [SerializeField] private PlayerMover _mover;
        
        [Header("Settings")]
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;
        [SerializeField] private float _maxSlidingTime;
        [SerializeField] private float _slidingColdown;
        [SerializeField] private float _jumpingColdown;

        public bool IsCriticalCondition = false;
        public bool IsJumping = false;
        public bool CanLand = false;
        public bool IsSliding = false;
        public bool IsRolling = false;
        public bool IsStumble = false;
        public bool IsFall = false;
        public bool Restart = false;
        public float CurrentCriticalCuldown = 0f;

        public delegate void OnGameOver();
        public OnGameOver OnGameOverEvent;

        private PlayerStateMachine _stateMachine;

        private void Awake() => InitStateMachine();

        public float MaxX => _maxX;
        public float MinX => _minX;
        public Animator ColliderAnimator => _colliderAnimator;
        public float CriticalCooldown { get; } = 3;

        public CameraController CameraController => _cameraController;

        private void Update()
        {
            _stateMachine.Update();

            if (IsCriticalCondition)
            {
                if (CurrentCriticalCuldown > 0) CurrentCriticalCuldown -= Time.deltaTime;
                else
                {
                    IsCriticalCondition = false;
                    _cameraController.DisableyDamageEffect();
                }
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

            Transition[] transitions = new Transition[] {
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

                //Stumble
                new Transition(stumble, fall, CanFall),
                new Transition(stumble, run, CanRun),

                //Fall
                new Transition(fall, run, CanRestart),
            };

            _stateMachine = new PlayerStateMachine(states, transitions, run);
        }

        private bool CanRun() => _mover.IsGraunded && !IsSliding && !IsRolling && CanLand;
        private bool CanSliding() => _mover.IsGraunded && !IsSliding && !IsRolling && !IsJumping && _mover.SlidingInput;
        private bool CanJump() => !IsJumping && !IsRolling && _mover.JumpInput && _mover.IsGraunded;
        private bool CanRoll() => IsJumping && _mover.SlidingInput && !IsRolling && !_mover.IsGraunded;
        private bool CanStumble() => _mover.IsGraunded && IsStumble;
        private bool CanFall() => IsFall;
        private bool CanRestart() => Restart && !IsFall;
    }
}
