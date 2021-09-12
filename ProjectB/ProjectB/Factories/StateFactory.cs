using ProjectB.Models.States.Enums;
using ProjectB.Services;
using ProjectB.States;

namespace ProjectB.Factories
{
    public class StateFactory : IStateFactory
    {
        //private readonly IStateProviderService stateProvider;

        //public StateFactory(IStateProviderService stateProvider)
        //{
        //    this.stateProvider = stateProvider;
        //}

        public IState GetState(ContextState state)
        {
            var result = state switch
            {
                ContextState.MainState or _ => (IState)new MainState(),
            };

            return result;
        }
    }
}
