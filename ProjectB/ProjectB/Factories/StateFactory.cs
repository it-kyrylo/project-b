using ProjectB.Enums;
using ProjectB.Services;
using ProjectB.States;

namespace ProjectB.Factories
{
    public class StateFactory : IStateFactory
    {
        private IHotelService _hotelService;

        public StateFactory(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public IState GetState(State state)
        {
            var result = state switch
            {
                State.HelpState => new HelpState(),
                State.CheckOutSelectState => new CheckOutSelectState(),
                State.CheckOutState => new CheckOutState(),
                State.CheckInSelectState => new CheckInSelectState(),
                State.CheckInState => new CheckInState(),
                State.HotelSelectState => new HotelSelectState(),
                State.CityTypedFromUserState => new CityTypedFromUser(_hotelService),
                State.CitySelectState => new CitySelectState(),
                State.MainState or _=> (IState)new MainState(),
            };

            return result;
        }
    }
}
