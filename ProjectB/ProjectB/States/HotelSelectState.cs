namespace ProjectB.States;

public class HotelSelectState : IState
{
    private ICosmosDbService<UserInformation> _cosmosDbService;

    public HotelSelectState(ICosmosDbService<UserInformation> cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;
    }

    public async Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
    {
        var userInforamation = new UserInformation();
        userInforamation.Id = callbackQuery.Message.Chat.Id.ToString();
        userInforamation.HotelId = callbackQuery.Data.ToString();
        await _cosmosDbService.AddHotelIdAsync(userInforamation);
        await BotSendMessage(botClient, callbackQuery.Message.Chat.Id);
        return State.HotelSelectState;
    }

    public async Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
    => await Task.FromResult(State.HotelSelectState);

    public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
                // first row
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Show CheckIn Date"),
                }
        });

        await botClient.SendTextMessageAsync(chatId, "Select", replyMarkup: inlineKeyboard);
    }
}
