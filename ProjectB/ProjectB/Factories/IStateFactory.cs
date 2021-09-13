using ProjectB.Enums;
using ProjectB.States;

namespace ProjectB.Factories
{
    public interface IStateFactory
    {
        IState GetState(State state);
    }
}
