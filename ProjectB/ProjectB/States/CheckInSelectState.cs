using ProjectB.Enums;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectB.States
{
    public class CheckInSelectState : IState
    {
        public Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            var checkIn = callbackQuery.Data.ToString();
            return Task.FromResult(State.CheckInSelectState);
        }

        public Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        => Task.FromResult(State.CheckInSelectState);

        public Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        => Task.FromResult(State.CheckInSelectState);
    }
}
