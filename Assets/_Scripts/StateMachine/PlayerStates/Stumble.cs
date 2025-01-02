using _Scripts.StateMachine.Abstractions;
using _Scripts.StateMachine.Interfaces;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerStates
{
    public class Stumble : State, IEnterState, IUpdateState
    {
        private readonly float _speed = 8f;
        private readonly float _timer = 0.2f;
        private float _currentTimer = 0.2f;
        private bool _timerActive = false;

        public Stumble(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _controller.IsStumble = false;

            _controller.CurrentCriticalCuldown = _controller.CriticalCooldown;
            _controller.IsCriticalCondition = true;
            _controller.CameraController.ApplyDamageEffect();
            
            _controller.CanLand = false;
            _timerActive = true;

            if (previous is Run) _animator.SetTrigger(PlayerAnimationTriggers.Stumble.ToString());
        }

        public void Update()
        {
            _mover.Move(_controller.MinX, _controller.MaxX, _speed);
            _mover.UpdateIsGraunded();

            if (!_timerActive) return;
            
            if (_currentTimer <= 0)
            {
                _timerActive = false;
                _controller.CanLand = true;
                _currentTimer = _timer;
            }
            else _currentTimer -= Time.deltaTime;
        }
    }
}
