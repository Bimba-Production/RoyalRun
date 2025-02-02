using _Scripts.Audio;
using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerStates
{
    public sealed class Roll : State, IEnterState, IUpdateState, IExitState
    {
        public StateNames  Name { get; set;} = StateNames.Roll;
        
        private readonly float _force = 16f;
        private readonly float _timer = 0.3f;
        private float _completeCooldownTime = 0f;
        private bool _timerActive = false;

        public Roll(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            AudioEffectController.Instance.Play(AudioEffectNames.roll, PlayerMover.Instance.transform.position);
            
            _completeCooldownTime = Time.realtimeSinceStartup + _timer;
            _controller.ResetAllTriggers();
            _mover.UpdateIsGrounded();
            _mover.ApplyForce(Vector3.down, _force);

            _controller.ColliderAnimator.SetTrigger(ColliderAnimationTrigger.Sit.ToString());
            if (previous.Name == StateNames.Jump) _animator.SetTrigger(PlayerAnimationTriggers.Roll.ToString());

            _controller.IsJumping = false;
            _controller.IsRolling = true;
            _timerActive = true;
        }

        public void Update()
        {
            _mover.Move();
            
            if (_timerActive && Time.realtimeSinceStartup >= _completeCooldownTime)
            {
                _timerActive = false;
                _controller.CanLand = true;
                _controller.IsRolling = false;
                _completeCooldownTime = 0;
            }
            else _mover.UpdateIsGrounded();
        }

        public void Exit()
        {
            _controller.IsRolling = false;
            _controller.CanLand = false;
            _controller.ColliderAnimator.SetTrigger(ColliderAnimationTrigger.Stand.ToString());
        }
    }
}
