﻿namespace Assets._Scripts.StateMachine
{
    public interface IState {}

    public interface IEnterState : IState
    {
        public void Enter(IState previous);
    }

    public interface IExitState : IState
    {
        public void Exit();
    }

    public interface IUpdateState : IState
    {
        public void Update();
    }

    public interface IFixedUpdateState : IState
    {
        public void FixedUpdate();
    }
}