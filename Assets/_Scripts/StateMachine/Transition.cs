using System;

namespace Assets._Scripts.StateMachine
{
    public class Transition
    {
        public IState From { get; }

        public IState To { get; }
        public Func<bool> Condition { get; }
        public Transition(IState from, IState to, Func<bool> condition)
        {
            From = from;
            To = to;
            Condition = condition;
        }
    }
}
