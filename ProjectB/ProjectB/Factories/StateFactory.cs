namespace ProjectB.Factories;

public class StateFactory : IStateFactory
{
    private IHotelService _hotelService;
    private ICosmosDbService<UserInformation> _cosmosDbService;

    public StateFactory(IHotelService hotelService, ICosmosDbService<UserInformation> cosmosDbService)
    {
        _hotelService = hotelService;
        _cosmosDbService = cosmosDbService;
    }

    public IState GetState(State state)
    {
        var result = state switch
        {
            State.HotelInfoState => new HotelInfoState(_hotelService, _cosmosDbService),
            State.HelpState => new HelpState(),
            State.CheckOutSelectState => new CheckOutSelectState(_cosmosDbService),
            State.CheckOutState => new CheckOutState(),
            State.CheckInSelectState => new CheckInSelectState(_cosmosDbService),
            State.CheckInState => new CheckInState(),
            State.HotelSelectState => new HotelSelectState(_cosmosDbService),
            State.CityTypedFromUserState => new CityTypedFromUser(_hotelService,_cosmosDbService),
            State.CitySelectState => new CitySelectState(_cosmosDbService),
            State.MainState or _=> (IState)new MainState(),
        };

        return result;
    }
}
