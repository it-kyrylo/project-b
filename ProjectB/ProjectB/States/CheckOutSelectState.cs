using ProjectB.Enums;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectB.States
{
    public class CheckOutSelectState : IState
    {
        public Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            var chechOutDate = callbackQuery.Data.ToString();
            return Task.FromResult(State.CheckOutSelectState);
        }

        public Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        => Task.FromResult(State.CheckOutSelectState);

        public Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        => Task.FromResult(State.CheckOutSelectState);
    }
}
