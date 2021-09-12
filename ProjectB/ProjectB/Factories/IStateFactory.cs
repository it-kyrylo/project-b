using ProjectB.Models.States.Enums;
using ProjectB.States;
using System;

namespace ProjectB.Factories
{
    public interface IStateFactory
    {
        IState GetState(ContextState state);
    }
}
