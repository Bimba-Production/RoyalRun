using UnityEngine;

namespace Assets._Scripts.StateMachime
{
    public class Fall : State, IEnterState
    {
        public Fall(Animator animator, PlayerMover mover, PlayerController controller) : base(animator, mover, controller)
        {
        }

        public void Enter(IState previous)
        {
            _animator.SetTrigger("Fall");
        }
    }
}
