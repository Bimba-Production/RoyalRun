using UnityEngine;

namespace Assets._Scripts.StateMachine
{
    public class Sliding : State, IEnterState, IUpdateState, IExitState
    {
        private float _speed = 8f;
        private float _timer = 0.8f;
        private float _currntTimer = 0.8f;
        private bool _timerActive = false;

        public Sliding(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            if (previous is Run) _animator.SetTrigger("RunToSliding");

            _controller.CanLand = false;
            _controller.IsSliding = true;
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
                    _controller.IsSliding = false;
                    _controller.CanLand = true;
                    _currntTimer = _timer;
                }
                else _currntTimer -= Time.deltaTime;
            }
        }

        public void Exit()
        {
            _controller.IsSliding = false;
        }
    }
}
