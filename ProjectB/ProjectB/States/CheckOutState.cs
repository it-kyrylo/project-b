namespace ProjectB.States;

public class CheckOutState : IState
{
    public async Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
    {
        await BotSendMessage(botClient, callbackQuery.Message.Chat.Id);
        return State.CheckOutState;
    }

    public async Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
    => await Task.FromResult(State.CheckOutState);

    public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
    {
        var buttons = new List<InlineKeyboardButton[]>();
        var choosedate = DateTime.UtcNow;
        var dayscount = 1;
        for (int i = 1; i <= 7; i++)
        {
            choosedate = choosedate.AddDays(dayscount);
            var button = new[]
            {
                InlineKeyboardButton.WithCallbackData(choosedate.ToString("yyyy-MM-dd"),choosedate.ToString("yyyy-MM-dd"))
            };

            buttons.Add(button);
        }
        var inlineKeyboard = new InlineKeyboardMarkup(buttons);

        var message = new Message();
        message.Text = "Choose check out date";

        await botClient.SendTextMessageAsync(chatId, message.Text, replyMarkup: inlineKeyboard);
    }
}
