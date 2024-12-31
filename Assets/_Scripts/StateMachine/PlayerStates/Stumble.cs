using UnityEngine;

namespace Assets._Scripts.StateMachine
{
    public class Stumble : State, IEnterState, IUpdateState
    {
        private float _speed = 8f;

        private float _timer = 0.2f;
        private float _currntTimer = 0.2f;
        private bool _timerActive = false;

        public Stumble(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _controller.IsStumble = false;

            _controller.CurrentCriticalCuldown = _controller.CriticalCuldown;
            _controller.IsCriticalCondition = true;
            _controller.CameraController.ApplyDamageEffect();
            
            _controller.CanLand = false;
            _timerActive = true;

            if (previous is Run) _animator.SetTrigger("Stumble");
        }

        public void Update()
        {
            _mover.Move(_controller.MinX, _controller.MaxX, _speed);
            _mover.UpdateIsGraunded();

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
        }
    }
}
