namespace ProjectB.States;

public class HelpState : IState
{
    public async Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
    {
        await BotSendMessage(botClient, callbackQuery.Message.Chat.Id);
        return State.HelpState;
    }

    public Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
    => Task.FromResult(State.HelpState);

    public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Back","Back"),
            },
        });
        var message = new Message();
        message.Text = "Back To MainMenu";

        await botClient.SendTextMessageAsync(chatId, "Welcome We Are Here To Help U Choose You'r Hotel For Your Vacation", replyMarkup: inlineKeyboard);
    }
}
