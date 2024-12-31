using UnityEngine;

namespace Assets._Scripts.StateMachine
{
    public class Roll : State, IEnterState, IUpdateState, IExitState
    {
        private float _speed = 8f;
        private float _gravityForce = 66f;
        private float _timer = 1f;
        private float _currntTimer = 1f;
        private bool _timerActive = false;

        public Roll(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _mover.AppluForce(Vector3.down, 8f);

            if (previous is Jump) _animator.SetTrigger("Roll");

            _controller.IsJumping = false;
            _controller.IsRolling = true;
            _timerActive = true;
        }

        public void Update()
        {
            _mover.Move(_controller.MinX, _controller.MaxX, _speed);

            if (_timerActive)
            {
                if (_currntTimer <= 0)
                {
                    _timerActive = false;
                    _controller.CanLand = true;
                    _controller.IsRolling = false;
                    _currntTimer = _timer;
                }
                else _currntTimer -= Time.deltaTime;
            }
            else _mover.UpdateIsGraunded();
        }

        public void Exit()
        {
            _controller.IsRolling = false;
        }
    }
}
