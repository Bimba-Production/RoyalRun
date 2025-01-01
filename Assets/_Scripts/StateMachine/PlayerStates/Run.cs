using UnityEngine;

namespace Assets._Scripts.StateMachine
{
    public class Run : State, IEnterState, IUpdateState
    {
        private readonly float _speed = 8f;

        public Run(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            if (previous is Jump)_animator.SetTrigger(PlayerAnimationTriggers.Land.ToString());
            else if (previous is Sliding) _animator.SetTrigger(PlayerAnimationTriggers.SlidingToRun.ToString());
        }

        public void Update()
        {
            _mover.Move(_controller.MinX, _controller.MaxX, _speed);
            _mover.UpdateIsGraunded();
        }
    }
}
