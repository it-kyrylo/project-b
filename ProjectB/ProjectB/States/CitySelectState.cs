namespace ProjectB.States;

public class CitySelectState : IState
{
    private ICosmosDbService<UserInformation> _cosmosDbService;

    public CitySelectState(ICosmosDbService<UserInformation> cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;
    }

    public async Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
    {
        await BotSendMessage(botClient, callbackQuery.Message.Chat.Id);

        return State.CitySelectState;
    }

    public async Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
    => await Task.FromResult(State.CitySelectState);

    public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
    {
        var searchHistory = await _cosmosDbService.GetHistoryAsync(chatId.ToString());
        if (searchHistory != null)
        {
            searchHistory = searchHistory.ToList();
            var buttons = new List<InlineKeyboardButton[]>();
            foreach (var item in searchHistory)
            {
                var button = new[]
                {
                InlineKeyboardButton.WithCallbackData(item,item)
                };
                buttons.Add(button);
            }

            var inlineKeyboard = new InlineKeyboardMarkup(buttons);

            await botClient.SendTextMessageAsync(chatId, "Please Type Or Choose The City You Wont To Visit", replyMarkup: inlineKeyboard);
        }
        else
        {
            await botClient.SendTextMessageAsync(chatId, "Please Type The City You Wont To Visit");
        }
    }
}
