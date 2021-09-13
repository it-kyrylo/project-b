using ProjectB.Enums;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectB.States
{
    public class CitySelectState : IState
    {
        public async Task<State> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await BotSendMessage(botClient, callbackQuery.Message.Chat.Id);
            
            return State.CitySelectState;
        }

        public Task<State> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        => Task.FromResult(State.CitySelectState);

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendTextMessageAsync(chatId, "Please Type The City You Wont To Visit");
        }
    }
}
