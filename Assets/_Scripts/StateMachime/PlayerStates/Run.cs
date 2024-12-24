using UnityEngine;

namespace Assets._Scripts.StateMachime
{
    public class Run : State, IEnterState, IUpdateState
    {
        private float _speed = 8f;

        public Run(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _mover.ResetYPos();

            if (previous is Jump)
            {
                _animator.SetTrigger("Land");
            }
            else if (previous is Sliding)
            {
                _animator.SetTrigger("SlidingToRun");
            }
        }

        public void Update()
        {
            _mover.Move(_controller.MinX, _controller.MaxX, _speed);
            _mover.UpdateIsGraunded();
        }
    }
}
