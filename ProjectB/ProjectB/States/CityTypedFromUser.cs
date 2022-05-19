namespace ProjectB.States;

public class CityTypedFromUser : IState
{
    private IHotelService _hotelService;
    private ICosmosDbService<UserInformation> _cosmosDbService;

    public CityTypedFromUser(IHotelService hotelService, ICosmosDbService<UserInformation> cosmosDbService)
    {
        _hotelService = hotelService;
        _cosmosDbService = cosmosDbService;
    }

    public async Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
    {
        await BotSendMessage(botClient, callbackQuery.Message.Chat.Id, callbackQuery.Data.ToString());
        return State.CityTypedFromUserState;
    }

    public async Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
    {
        var userInformation = new UserInformation();
        userInformation.Id = message.Chat.Id.ToString();
        var city = char.ToUpper(message.Text.ToString()[0]) + message.Text.ToString().Substring(1);
        await _cosmosDbService.AddToHistoryAsync(userInformation, city);
        await BotSendMessage(botClient, message.Chat.Id, message.Text.ToString());
        return State.CityTypedFromUserState;
    }

    

    public async Task BotSendMessage(ITelegramBotClient botClient, long chatId, string cityName)
    {
        var hotels = await _hotelService.GetDestinationIdAsync(cityName);
        var buttons = new List<InlineKeyboardButton[]>();
        foreach (var hotel in hotels)
        {
            var button = new[]
            {
                InlineKeyboardButton.WithCallbackData(hotel.HotelName,hotel.Id.ToString())
            };
            buttons.Add(button);
        }

        var inlineKeyboard = new InlineKeyboardMarkup(buttons);
        var message = new Message();
        message.Text = "Choose From Hotels";
        await botClient.SendTextMessageAsync(chatId, message.Text, replyMarkup: inlineKeyboard);
    }

    public Task BotSendMessage(ITelegramBotClient botClient, long chatId)
    => Task.FromResult(State.CityTypedFromUserState);
}
