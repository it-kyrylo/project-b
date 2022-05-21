namespace ProjectB.Factories;

public class StateFactory : IStateFactory
{
    private IHotelService _hotelService;
    private ICosmosDbService<UserInformation> _cosmosDbService;
    private readonly ICacheFilter<string> _cacheFilter;

    public StateFactory(IHotelService hotelService, ICosmosDbService<UserInformation> cosmosDbService,
        ICacheFilter<string> cacheFilter)
    {
        _hotelService = hotelService;
        _cosmosDbService = cosmosDbService;
        _cacheFilter = cacheFilter;
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
            State.CityTypedFromUserState => new CityTypedFromUser(_hotelService, _cosmosDbService, _cacheFilter),
            State.CitySelectState => new CitySelectState(_cosmosDbService),
            State.MainState or _ => (IState)new MainState(),
        };

        return result;
    }
}
