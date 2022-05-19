namespace ProjectB.Factories;

public interface IStateFactory
{
    IState GetState(State state);
}
