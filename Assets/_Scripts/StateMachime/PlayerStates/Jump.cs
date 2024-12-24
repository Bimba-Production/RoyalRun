using UnityEngine;

namespace Assets._Scripts.StateMachime
{
    public class Jump : State, IEnterState, IUpdateState, IExitState
    {
        private float _speed = 8f;
        private float _gravityForce = 22f;
        private float _jumpForce = 11f;

        private float _timer = 0.6f;
        private float _currntTimer = 0.6f;
        private bool _timerActive = false;

        public Jump(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            if (previous is Run
                || previous is Sliding) _animator.SetTrigger("Jump");

            _mover.AppluForce(_jumpForce);

            _mover.IsGraunded = false;
            _controller.IsJumping = true;
            _timerActive = true;
        }

        public void Update()
        {
            _mover.AplyGravity(_gravityForce);
            _mover.Move(_controller.MinX, _controller.MaxX, _speed);

            if (_timerActive)
            {
                if (_currntTimer <= 0)
                {
                    _timerActive = false;
                    _controller.CanLand = true;
                    _currntTimer = _timer;
                }
                else _currntTimer -= Time.deltaTime;
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
