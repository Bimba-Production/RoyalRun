using UnityEngine;

namespace Assets._Scripts.StateMachine
{
    public class Jump : State, IEnterState, IUpdateState, IExitState
    {
        private readonly float _speed = 8f;
        private readonly float _jumpForce = 10f;
        private readonly float _timer = 0.6f;
        private float _currentTimer = 0.6f;
        private bool _timerActive = false;

        public Jump(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _mover.ApplyForce(Vector3.up, _jumpForce);
            
            if (previous is Run
                || previous is Sliding) _animator.SetTrigger(PlayerAnimationTriggers.Jump.ToString());
            
            _mover.IsGraunded = false;
            _controller.IsJumping = true;
            _timerActive = true;
        }

        public void Update()
        {
            _mover.Move(_controller.MinX, _controller.MaxX, _speed);

            if (_timerActive)
            {
                if (_currentTimer <= 0)
                {
                    _timerActive = false;
                    _controller.CanLand = true;
                    _currentTimer = _timer;
                }
                else _currentTimer -= Time.deltaTime;
            }
            else _mover.UpdateIsGraunded();
        }

        public void Exit()
        {
            _controller.IsJumping = false;
            _controller.CanLand = false;
            _mover.IsGraunded = true;
        }
    }
}
